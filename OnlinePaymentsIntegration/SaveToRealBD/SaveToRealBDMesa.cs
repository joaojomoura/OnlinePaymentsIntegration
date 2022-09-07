using OnlinePaymentsIntegration.SIBS.SDK;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace OnlinePaymentsIntegration.SaveToRealBD
{
    public class SaveToRealBDMesa
    {
        private SqlConnection sqlcon;
        private SqlConnection sqlcon1;
        private ResponseRequestCopyAndPay responseRequest;
        private Dictionary<string, dynamic> getResponse;
        private string connection = @"Data Source=sql.inovanet.pt,3433;Initial Catalog=pizzarte_testes;User ID=pizzartenet;Password=qjaabsuf6969$;encrypt=true;trustServerCertificate=true";

        public void checkTimeOfPendingTransaction() {
            var URL = "https://spg.qly.site1.sibs.pt";
            DateTime presentTime = DateTime.Now;
            
            //START CONNECTION TO BD
            try {
                sqlcon = new SqlConnection(connection);
                sqlcon.Open();
            }
            catch {
                sqlcon.Close();
            }
            DateTime expireDate = new DateTime();
            string transactionId = "";

            // GET THE PENDING TRANSACTIONS
            string query = @"SELECT * FROM TRANSACTIONS WITH (NOLOCK) WHERE Payment_Status = 'Pending'";
            SqlDataReader sqlDR = null;
            try {
                SqlCommand SqlExecute0 = new SqlCommand(query, sqlcon);
                sqlDR = SqlExecute0.ExecuteReader();
                while (sqlDR.Read()) {
                    expireDate = Convert.ToDateTime(sqlDR.GetString(sqlDR.GetOrdinal("Execution_End_Time"))).AddMinutes(5);
                    transactionId = sqlDR.GetString(sqlDR.GetOrdinal("TransactionId"));
                    if (sqlDR.GetString(sqlDR.GetOrdinal("Payment_Method")).Equals("REFERENCE")) {
                        expireDate = Convert.ToDateTime(sqlDR.GetString(sqlDR.GetOrdinal("MBREF_ExpireDate")));
                    }
                    // IF THE TIME OF EXPIRE DATE IS LESSER THAN THE PRESENT GONNA UPDATE THE STATUS
                    if(expireDate.CompareTo(presentTime) <= 0) {
                        responseRequest = new ResponseRequestCopyAndPay(URL, TransactionDataForForm.clientIdOnSibs, TransactionDataForForm.bearer, transactionId);
                        getResponse = responseRequest.getResponseRequest();
                        
                        string updateTransactionAfterTimeExpired = "UPDATE TRANSACTIONS SET Payment_Status = '" +
                            getResponse["paymentStatus"] + "' WHERE TransactionId = '" + transactionId + "'";
                        sqlcon1 = new SqlConnection(connection);
                        sqlcon1.Open();
                        SqlTransaction transactionlocal = sqlcon1.BeginTransaction();
                        SqlDataReader sqlDR1 = null;
                        try {
                            SqlCommand SqlExecute1 = new SqlCommand(updateTransactionAfterTimeExpired, sqlcon1, transactionlocal);
                            sqlDR1 = SqlExecute1.ExecuteReader();
                            sqlDR1.Close();
                            if ((transactionlocal != null))
                                transactionlocal.Commit();
                            sqlcon1.Close();
                        }
                        catch (SqlException ex) {
                            if (sqlDR1 != null) {
                                if (sqlDR1.IsClosed == false)
                                    sqlDR1.Close();
                            }
                            transactionlocal.Rollback();
                            sqlcon.Close();
                            sqlcon1.Close();

                        }
                     //////////////////////////////////////////////////
                    }
                }
                sqlDR.Close();
            }
            catch {
                if (sqlDR != null) {
                    if (sqlDR.IsClosed == false)
                        sqlDR.Close();
                }
                sqlcon.Close();
                
            }
            sqlcon.Close();
            
        }

        private void saveFromCopyBDtoRealBD(DataTable basketDetails, string transactionId, object amount, object payment_Method,
            object orderTime, object codigoCliente, object mailBasketBody, object observations) {
            string sSqlCod = string.Empty;
            SqlDataReader sqlDrCod = null;

            string sMesa = "";
            SqlConnection sqlconCli = new SqlConnection(connection);
            sqlconCli.Open();

            SqlTransaction transaction = null;
            transaction = sqlconCli.BeginTransaction();


            SqlConnection sqlcon = new SqlConnection(connection);
            sqlcon.Open();
            SqlTransaction transactionlocal = null;
            transactionlocal = sqlcon.BeginTransaction();

            try {
                #region LANÇAMENTO FINAL NA MESA
                string sdescricao = "";
                string TempQuery = string.Empty;

                string querymesas = string.Empty;
                querymesas = "SELECT codigo, descricao FROM IStk47000 WITH (NOLOCK) WHERE WEB=1"; //WHERE codigo='zzz'";// WHERE codigo not in('100','101','102','105') //FALTA retirar codigo do ZZZ

                SqlCommand SqlExecute0 = new SqlCommand(querymesas, sqlconCli, transaction);
                SqlDataReader sqlDRmesas = SqlExecute0.ExecuteReader();
                bool encontroumesa = false;

                while (sqlDRmesas.Read()) {
                    if (encontroumesa == false) {
                        sMesa = sqlDRmesas.GetString(sqlDRmesas.GetOrdinal("codigo"));
                        sdescricao = sqlDRmesas.GetString(sqlDRmesas.GetOrdinal("descricao"));

                        if (sMesa != "") {
                            //verifica se mesa já tem dados - se sim, é porque está a ser usada, sendo assim sai-o para ir procurar outra mesa vaga 
                            TempQuery = "";
                            TempQuery = "SELECT TOP 1 Mesa FROM IStk48000 WHERE Mesa='" + sMesa + "'";

                            //sqlcon.Open();
                            SqlCommand SqlExecute1 = new SqlCommand(TempQuery, sqlcon, transactionlocal);
                            SqlDataReader sqlDR0 = SqlExecute1.ExecuteReader();
                            if (sqlDR0.Read()) {
                                if (sqlDR0["Mesa"] != System.DBNull.Value) {
                                    sqlDR0.Close();
                                    //sqlcon.Close();
                                    sMesa = "";
                                    encontroumesa = false;
                                    //vou procurar uma nova mesa
                                }
                            }
                            else {
                                sqlDR0.Close();
                                // sqlcon.Close();
                                TempQuery = "";
                                TempQuery = "SELECT (SELECT descricao FROM IStk47000 WITH (NOLOCK) WHERE codigo='" + sMesa + "') AS NomeMesa, ";
                                TempQuery += "(SELECT substring(column_name, CHARINDEX('_',column_name)+1, LEN(column_name)) FROM ";
                                TempQuery += "tempdb.information_schema.key_column_usage WHERE TABLE_NAME = ";
                                TempQuery += "'##" + NomeTabelaTemporaria(9999, sMesa) + "') as Caixa, ";
                                TempQuery += "name FROM tempdb.dbo.sysobjects o WHERE o.xtype in ('U') ";
                                TempQuery += "AND o.id = object_id(N'tempdb..[##" + NomeTabelaTemporaria(9999, sMesa) + "]')";

                                //sqlcon.Open();
                                SqlCommand SqlExecute2 = new SqlCommand(TempQuery, sqlcon, transactionlocal);
                                SqlDataReader sqlDR = SqlExecute2.ExecuteReader();
                                if (sqlDR.Read()) {
                                    if (sqlDR["name"] != System.DBNull.Value) //ou !=""
                                    {
                                        //string name = sqlDR.GetString(sqlDR.GetOrdinal("name"));
                                        //se o nome existe é porque está ser usado, sendo assim sai-o para ir procurar outra mesa vaga
                                        sqlDR.Close();
                                        // sqlcon.Close();
                                        encontroumesa = false;
                                        //vou procurar uma nova mesa
                                    }
                                }
                                else {
                                    sqlDR.Close();
                                    //sqlcon.Close();

                                    // sqlcon.Open();
                                    // transaction = sqlcon.BeginTransaction();
                                    SqlDataReader sqlDR1 = null;

                                    string TempQueryCreate = "";
                                    TempQueryCreate = "CREATE TABLE ##" + NomeTabelaTemporaria(9999, sMesa) + " (" + "CAIXA_" + 9999 + " INT PRIMARY KEY)";

                                    //sqlDR1 = Data.Consulta(sqlcon, TempQueryCreate, true, transactionlocal);
                                    //sqlDR1.Close();
                                    //retirada transação para evitar bloqueio completo nas mesas
                                    SqlConnection sqlConnD = new SqlConnection(connection);
                                    sqlConnD.Open();
                                    SqlCommand SqlExecute3 = new SqlCommand(TempQueryCreate, sqlConnD);
                                    SqlDataReader sqlDR01 = SqlExecute3.ExecuteReader();
                                    sqlDR01.Close();

                                    encontroumesa = true;

                                    //lança artigos na mesa e hora do levantamento
                                    string sSqlCabecalho = string.Empty;
                                    sSqlCabecalho = "UPDATE IStk47000 SET HoraAbertura=FORMAT(CAST(GETDATE() AS DATETIME), 'HH:mm'), Activa=1,TotalMesa=" + (amount.ToString()).Replace(",", ".") + ",";
                                    //sSqlCabecalho += "Situacao=3,Zona=0,NrPessoas=0, cliente='" + lbidcliente + "', formapagamento='" + metododepagamento + "', DataHoraEntrega='"+ Utils.DataBD1(DateTime.Now)+" "+ horalevantamento + "' ";
                                    sSqlCabecalho += "Situacao=3,NrPessoas=0, cliente='" + codigoCliente + "', formapagamento='" + payment_Method + "', DataHoraEntrega='" + orderTime + "' ";
                                    sSqlCabecalho += "WHERE Codigo='" + sMesa + "' and descricao='" + sdescricao + "'";
                                    SqlExecute3 = new SqlCommand(sSqlCabecalho, sqlcon, transactionlocal);
                                    sqlDR1 = SqlExecute3.ExecuteReader();
                                    sqlDR1.Close();

                                    //copia de tempbd para real bd

                                    foreach (DataRow row in basketDetails.Rows) {
                                        string sSqlDetalhes = string.Empty;
                                        sSqlDetalhes = "INSERT INTO IStk48000(Mesa, Vendedor, Referencia, Quantidade, Hora, PrecoUnitarioComIva,";// Data,
                                        sSqlDetalhes += "PrecoUnitario, Marcado, TaxaIva, DescontoLinha, DescontoLinhaDefeito, DescontoMaximoArtigo,";
                                        sSqlDetalhes += "PrecoAgrupado, DescontoAgrupado, EstacaoSaldos, TipoDesconto, ArtigoKit, QuantidadeImpressa,";
                                        sSqlDetalhes += "ArtigoPeso, CodSincr, CodLinha, composto, TemPerfis, Com, Sem, Perfil, Predefinido, DescontoObrigatorio,";
                                        sSqlDetalhes += "ArtigoDescritivo, Descricao, Seccoes, NrConsulta, SerieConsulta, QtdConciliada,";
                                        sSqlDetalhes += "NrConsultaOrigem, SerieConsultaOrigem, IncluirExtrasNaFactura, Armazem, Preceito," +
                                            "Data, EspacoFiscal, TipoImposto, MotivoDesconto)";
                                        sSqlDetalhes += "VALUES('" + sMesa + "', " + row["Vendedor"] + ",'" + row["Referencia"] + "', " + row["Quantidade"] + ", '" + row["Hora"] +
                                            "', " + row["PrecoUnitarioComIva"] + ", " + row["PrecoUnitario"] + ", " + row["Marcado"] + ", " + row["TaxaIva"] +
                                            ", " + row["DescontoLinha"] + ", " + row["DescontoLinhaDefeito"] + ", " + row["DescontoMaximoArtigo"] + 
                                            ", " + row["PrecoAgrupado"] + ", " + row["DescontoAgrupado"] + ", " + row["EstacaoSaldos"] + "," + row["TipoDesconto"] + ", " + row["ArtigoKit"] + ", " + row["QuantidadeImpressa"] + 
                                            ", " + row["ArtigoPeso"] + ", '" + row["CodSincr"] + "', " + row["CodLinha"] + "', " + row["composto"] + ", " + row["TemPerfis"] + ", " + row["Com"] + ", " + row["Sem"] + ", " + row["Perfil"] + ", " + row["Predefinido"] + ", " + row["DescontoObrigatorio"] +
                                            ", " + row["ArtigoDescritivo"] + ", '" + row["Descricao"] + "', '" + row["Seccoes"] + "', " + row["NrConsulta"] + ", '" + row["SerieConsulta"] + "', " + row["QtdConciliada"] +
                                            ", " + row["NrConsultaOrigem"] + ", '" + row["SerieConsultaOrigem"] + "', " + row["IncluirExtrasNaFactura"] + ", " + row["Armazem"] + ", " + row["Preceito"] + 
                                            ",'" + row["Data"] + "', '" + row["EspacoFiscal"] + "', '" + row["TipoImposto"] + "', " + row["MotivoDesconto"] + ")";
                                        SqlExecute3 = new SqlCommand(sSqlDetalhes, sqlcon, transactionlocal);
                                        sqlDR1 = SqlExecute3.ExecuteReader();
                                        sqlDR1.Close();
                                    }
                                    //limpa temporaria
                                    string TempQuery1 = "";
                                    TempQuery1 = "DROP TABLE IF EXISTS ##" + NomeTabelaTemporaria(9999, sMesa);
                                    SqlExecute3 = new SqlCommand(TempQuery1, sqlcon, transactionlocal);
                                    sqlDR1 = SqlExecute3.ExecuteReader();
                                    sqlDR1.Close();
                                }
                            }
                        }
                    }
                }
                sqlDRmesas.Close();
                if ((transaction != null) && (transactionlocal != null)) {
                    transaction.Commit();
                    transactionlocal.Commit();



                    #endregion
                    //comprovativo para cliente
                    try {
                        if (sMesa != "") //lançou na mesa
                        {
                            MailMessage msg1 = null;
                            string sSql1 = string.Empty;
                            sSql1 = "SELECT Email FROM IStk470000 WITH (NOLOCK) WHERE cliente= " + codigoCliente;
                            SqlCommand SqlExecute3 = new SqlCommand(sSql1, sqlconCli);
                            SqlDataReader sqlDR1 = SqlExecute3.ExecuteReader();
                            
                            if (sqlDR1.Read()) {
                                if (sqlDR1["Email"] != System.DBNull.Value) {
                                    msg1 = new MailMessage(getEmailAdministradorSite(), sqlDR1.GetString(sqlDR1.GetOrdinal("email"))); //FROM e to
                                }
                            }
                            sqlDR1.Close();

                            
                            msg1.Subject = "Pizzarte";
                            msg1.IsBodyHtml = true;
                            msg1.Body = "<font face='Verdana, Arial, Helvetica, sans-serif' size='2' color='#000000'>";
                            msg1.Body += "<a href=http://shop.pizzarte.pt/images/logop.png><img src=http://shop.pizzarte.pt/images/logop.png width=200px></a><br/><br/><br/>";
                            msg1.Body += getClientName(codigoCliente) + ", obrigada(o) pela sua compra!<br/><br/>";
                            msg1.Body += "Por favor indique este código no ato de levantamento.<br><br><b>CÓDIGO ATRIBUIDO: " + sdescricao + "</b><br/><br/><br/>";
                            msg1.Body += "<b>DETALHE DO PEDIDO</b><br/>";
                            // msg1.Body += "Hora escolhida para levantamento: " + horalevantamento + "";
                            // if (horalevantamento != Lb_infohoraprevista.Text)
                            msg1.Body += "<br/>Hora prevista para entrega: " + orderTime + "";
                            msg1.Body += "<br/><br/>";
                            msg1.Body += mailBasketBody + "<br/>";
                            msg1.Body += "<b>Valor total:</b> " + amount + " €";
                            if (observations.ToString() != "")
                                msg1.Body += "<br/><br/><b>Observações:</b> " + observations;
                            msg1.Body += "<br/>" + "<br/><br/><b>Pizzarte</b><br /><br />R.Engº Von Haffe, 27<br />3800-177 Aveiro, Portugal<br /><br /><a href=mailto:pizzarte@pizzarte.com>pizzarte@pizzarte.com</a><br/>(+351) 234 427 103";
                            msg1.Body += "</font>";
                            msg1.BodyEncoding = Encoding.UTF8;
                            new SmtpClient().Send(msg1);
                        }
                        /*else //não tem mesa
                        {
                            if (Utils.bSiteAceitaCompraSemMesas == true) //envia pedido por email se true
                            {
                                MailMessage msg1 = null;
                                string sSql1 = string.Empty;
                                sSql1 = "SELECT Email FROM IStk470000 WITH (NOLOCK) WHERE cliente=" + Session["CodCliente"].ToString();
                                SqlDataReader sqlDR1 = Data.Consulta(sqlconCli, sSql1, false, null);
                                if (sqlDR1.Read()) {
                                    if (sqlDR1["Email"] != System.DBNull.Value) {
                                        msg1 = new MailMessage(getEmailAdministradorSite(), sqlDR1.GetString(sqlDR1.GetOrdinal("email"))); //FROM e to
                                    }
                                }
                                sqlDR1.Close();

                                Label Lb_nomecliente = (Label)Master.FindControl("Lb_Logado");
                                msg1.Subject = Utils.nomeloja;
                                msg1.IsBodyHtml = true;
                                msg1.Body = Utils.formatacaoemail;
                                msg1.Body += Utils.logotipoemail;
                                msg1.Body += Lb_nomecliente.Text + ", obrigada(o) pela sua compra!<br/><br/>";
                                msg1.Body += "<b>DETALHE DO PEDIDO</b><br/>";
                                //msg1.Body += "Hora escolhida para levantamento: " + horalevantamento + "";
                                // if (horalevantamento != Lb_infohoraprevista.Text)
                                msg1.Body += "<br/>Hora prevista para entrega: " + Lb_infohoraprevista.Text + "";
                                msg1.Body += "<br/><br/>";
                                msg1.Body += ConteudoHTML + "<br/>";
                                msg1.Body += "<b>Valor total:</b> " + Lb_precototal.Text + " " + Lb_eurofinal.Text;
                                if (Tb_comentarios.Text != "")
                                    msg1.Body += "<br/><br/><b>Observações:</b> " + Tb_comentarios.Text;
                                msg1.Body += "<br/>" + idioma.RetornaMensagem("Moradarodapeemail");
                                msg1.Body += Utils.rodapeemail;
                                msg1.BodyEncoding = Encoding.UTF8;
                                new SmtpClient().Send(msg1);
                            }
                        }*/
                    }
                    catch (Exception ex) {
                        //falha no envio
                    }
                    ////////////////////////////////////////
                }
                sqlcon.Close();
                sqlconCli.Close();

            }
            catch {
                transaction.Rollback();
                transactionlocal.Rollback();
                sqlcon.Close();
                sqlconCli.Close();

                //limpa temporaria
                SqlDataReader sqlDR1 = null;
                string TempQuery1 = "";
                TempQuery1 = "DROP TABLE IF EXISTS ##" + NomeTabelaTemporaria(9999, sMesa);
                SqlCommand SqlExecute3 = new SqlCommand(TempQuery1, sqlcon);
                sqlDR1 = SqlExecute3.ExecuteReader();
                sqlDR1.Close();
            }
            
        }

        private string NomeTabelaTemporaria(int iCaixa, string sMesa) {
            int i = 0;
            string sCodigoMesa = "";
            string sDB = "";

            //byte[] asciiBytes = Encoding.ASCII.GetBytes(sMesa);
            for (i = 0; i < sMesa.Length; i++) //for i := 1 to length(sMesa) do
            {

                string codigos = "012345467890abcdefghijklmnopqrstuvwxyz";
                string searchString = sMesa[i].ToString().ToLower();
                int startIndex = codigos.IndexOf(searchString);
                //if pos(LowerCase(sMesa[i]), '012345467890abcdefghijklmnopqrstuvwxyz') > 0 then
                //if("012345467890abcdefghijklmnopqrstuvwxyz".IndexOf(sMesa[i].ToString().ToLower())>=0)

                if (startIndex >= 0)
                    sCodigoMesa = sCodigoMesa + sMesa[i];
                else {
                    //sCodigoMesa:= sCodigoMesa + '_' + IntToStr(ord(sMesa[i])) + '_';
                    //sCodigoMesa = sCodigoMesa + "_" + asciiBytes[i] + "_";
                    sCodigoMesa = sCodigoMesa + "_" + ((int)(sMesa[i])).ToString() + "_";
                }
            }
            //nome bd -- PrincipalDataModule.MainServerMSConnection.Database
            SqlConnection sqlconCli = new SqlConnection(connection);
            string server = sqlconCli.Database;

            for (i = 0; i < server.Length; i++) {
                string codigos = "012345467890abcdefghijklmnopqrstuvwxyz";

                string searchString = server[i].ToString().ToLower();
                int startIndex = codigos.IndexOf(searchString);

                if (startIndex >= 0)
                    sDB = sDB + server[i];
                else
                    sDB = sDB + "_" + ((int)(server[i])).ToString() + "_";
            }
            return sDB + "_Mesa_" + sCodigoMesa;
        }


        public void forEachTransactionNotProcessed() {
            SqlConnection sqlGetTransactionsCon = new SqlConnection(connection);
            SqlConnection updateTransactionCon = new SqlConnection(connection);
            sqlGetTransactionsCon.Open();
            string getTransactionsNotProcessed = "SELECT TransactionId FROM TRANSACTIONS WITH (NOLOCK) WHERE Processed = 0";
            SqlDataReader sqlReadTransactions = null;
            SqlCommand sqlExecuteTransactions = new SqlCommand(getTransactionsNotProcessed,sqlGetTransactionsCon);
            try {
                sqlReadTransactions = sqlExecuteTransactions.ExecuteReader();
                while (sqlReadTransactions.Read()) {
                    string transactionId = sqlReadTransactions.GetString(sqlReadTransactions.GetOrdinal("TransactionId"));
                    var amount = sqlReadTransactions.GetValue(sqlReadTransactions.GetOrdinal("Amount_Value"));
                    var payment_Method = sqlReadTransactions.GetValue(sqlReadTransactions.GetOrdinal("Payment_Method"));
                    var orderTime = sqlReadTransactions.GetValue(sqlReadTransactions.GetOrdinal("DataHoraEntrega"));
                    var mailBasketBody = sqlReadTransactions.GetValue(sqlReadTransactions.GetOrdinal("Mail_Basket_Body"));
                    var observations = sqlReadTransactions.GetValue(sqlReadTransactions.GetOrdinal("Observations"));
                    DataTable basket = basketDetails(transactionId);
                    var codigoCliente = basket.Rows[1]["CodigoCliente"];
                    saveFromCopyBDtoRealBD(basket,transactionId,amount,payment_Method,orderTime,codigoCliente, mailBasketBody,observations);
                    SqlDataReader updateProcessed = null;
                    try {
                        updateTransactionCon.Open();
                        string update = "UPDATE TRANSACTIONS SET Processed = 1 WHERE TransactionId = " + transactionId;
                        SqlCommand sqlExcuteUpdate = new SqlCommand(update,updateTransactionCon);
                        updateProcessed = sqlExcuteUpdate.ExecuteReader();
                        updateProcessed.Close();
                        updateTransactionCon.Close();
                    }
                    catch {
                        if (updateProcessed != null)
                            if (!updateProcessed.IsClosed)
                                updateProcessed.Close();
                        updateTransactionCon.Close();
                    }
                }
                sqlReadTransactions.Close();
                sqlGetTransactionsCon.Close();
            }
            catch {
                if (sqlReadTransactions != null) {
                    if (sqlReadTransactions.IsClosed == false)
                        sqlReadTransactions.Close();
                }
                sqlGetTransactionsCon.Close();
            }

        }

        public DataTable basketDetails (string transactionId) {
            SqlConnection sqlGeBasketCon = new SqlConnection(connection);
            sqlGeBasketCon.Open();
            string getBasket = "SELECT * FROM IStk48000_Copy WITH (NOLOCK) WHERE TransactionId = '" + transactionId + "'"; ;
            SqlDataReader sqlReadBasket = null;
            SqlCommand sqlExecuteTransactions = new SqlCommand(getBasket, sqlGeBasketCon);
            DataTable basket = CriaDataTable();
            
            try {
                sqlReadBasket = sqlExecuteTransactions.ExecuteReader();
                while (sqlReadBasket.Read()) {
                    DataRow row = basket.NewRow();
                    row["CodigoCliente"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("Pizzarte_Client_Code"));
                    row["Referencia"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("Referencia"));
                    row["Quantidade"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("Quantidade"));
                    row["PrecoUnitarioComIva"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("PrecoUnitarioComIva"));
                    row["PrecoUnitario"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("PrecoUnitario"));
                    //row["PrecoUnitarioComIvaCompleto"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("PrecoUnitarioComIvaCompleto"));
                    row["TaxaIva"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("TaxaIva"));
                    row["DescontoLinha"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("DescontoLinha"));
                    row["DescontoMaximoArtigo"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("DescontoMaximoArtigo"));
                    row["ArtigoKit"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("ArtigoKit"));
                    row["ArtigoPeso"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("ArtigoPeso"));
                    row["CodSincr"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("CodSincr"));
                    row["CodLinha"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("CodLinha"));
                    row["composto"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("composto"));
                    row["TemPerfis"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("TemPerfis"));
                    row["Com"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("Com"));
                    row["Sem"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("Sem"));
                    row["Perfil"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("Perfil"));
                    row["Predefinido"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("Predefinido"));
                    row["Descricao"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("Descricao"));
                    row["Seccoes"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("Seccoes"));
                    row["IncluirExtrasNaFactura"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("IncluirExtrasNaFactura"));
                    row["Armazem"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("Armazem"));
                    row["Preceito"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("Preceito"));
                    row["Vendedor"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("Vendedor"));
                    row["Hora"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("Hora"));
                    row["Data"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("Data"));
                    row["Marcado"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("Marcado"));
                    row["DescontoLinhaDefeito"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("DescontoLinhaDefeito"));
                    row["PrecoAgrupado"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("PrecoAgrupado"));
                    row["DescontoAgrupado"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("DescontoAgrupado"));
                    row["EstacaoSaldos"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("EstacaoSaldos"));
                    row["TipoDesconto"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("TipoDesconto"));
                    row["QuantidadeImpressa"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("QuantidadeImpressa"));
                    row["DescontoObrigatorio"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("DescontoObrigatorio"));
                    row["ArtigoDescritivo"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("ArtigoDescritivo"));
                    row["NrConsulta"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("NrConsulta"));
                    row["SerieConsulta"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("SerieConsulta"));
                    row["QtdConciliada"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("QtdConciliada"));
                    row["NrConsultaOrigem"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("NrConsultaOrigem"));
                    row["SerieConsultaOrigem"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("SerieConsultaOrigem"));
                    row["EspacoFiscal"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("EspacoFiscal"));
                    row["TipoImposto"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("TipoImposto"));
                    row["MotivoDesconto"] = sqlReadBasket.GetValue(sqlReadBasket.GetOrdinal("MotivoDesconto"));
                    basket.Rows.Add(row);

                }
                sqlReadBasket.Close();
                sqlGeBasketCon.Close();
            }
            catch {
                if (sqlReadBasket != null) {
                    if (sqlReadBasket.IsClosed == false)
                        sqlReadBasket.Close();
                }
                sqlGeBasketCon.Close();
            }

            return basket;
        }

        private DataTable CriaDataTable() {
            DataTable minhaTabela = new DataTable();
            minhaTabela.Columns.Add("CodigoCliente", Type.GetType("System.Int32"));//????
            minhaTabela.Columns.Add("Referencia", Type.GetType("System.String"));
            minhaTabela.Columns.Add("Quantidade", Type.GetType("System.Decimal"));
            minhaTabela.Columns.Add("PrecoUnitarioComIva", Type.GetType("System.Decimal"));
            minhaTabela.Columns.Add("PrecoUnitario", Type.GetType("System.Decimal"));
            minhaTabela.Columns.Add("PrecoUnitarioComIvaCompleto", Type.GetType("System.Decimal"));
            minhaTabela.Columns.Add("TaxaIva", Type.GetType("System.Decimal"));
            minhaTabela.Columns.Add("DescontoLinha", Type.GetType("System.Decimal"));
            minhaTabela.Columns.Add("DescontoMaximoArtigo", Type.GetType("System.Decimal"));
            minhaTabela.Columns.Add("ArtigoKit", Type.GetType("System.Int32"));
            minhaTabela.Columns.Add("ArtigoPeso", Type.GetType("System.Int32"));
            minhaTabela.Columns.Add("CodSincr", Type.GetType("System.String"));
            minhaTabela.Columns.Add("CodLinha", Type.GetType("System.String"));
            minhaTabela.Columns.Add("composto", Type.GetType("System.Int32"));
            minhaTabela.Columns.Add("TemPerfis", Type.GetType("System.Int32"));
            minhaTabela.Columns.Add("Com", Type.GetType("System.Int32"));
            minhaTabela.Columns.Add("Sem", Type.GetType("System.Int32"));
            minhaTabela.Columns.Add("Perfil", Type.GetType("System.Int32"));
            minhaTabela.Columns.Add("Predefinido", Type.GetType("System.Int32"));
            minhaTabela.Columns.Add("Descricao", Type.GetType("System.String"));
            minhaTabela.Columns.Add("Seccoes", Type.GetType("System.String"));
            minhaTabela.Columns.Add("IncluirExtrasNaFactura", Type.GetType("System.Int32"));
            minhaTabela.Columns.Add("Armazem", Type.GetType("System.Int32"));
            minhaTabela.Columns.Add("Preceito", Type.GetType("System.Int32"));
            //////
            minhaTabela.Columns.Add("Vendedor", Type.GetType("System.Int32"));
            minhaTabela.Columns.Add("Hora", Type.GetType("System.String"));
            minhaTabela.Columns.Add("Data", Type.GetType("System.String"));
            minhaTabela.Columns.Add("Marcado", Type.GetType("System.Int32"));
            minhaTabela.Columns.Add("DescontoLinhaDefeito", Type.GetType("System.Decimal"));
            minhaTabela.Columns.Add("PrecoAgrupado", Type.GetType("System.Decimal"));
            minhaTabela.Columns.Add("DescontoAgrupado", Type.GetType("System.Decimal"));
            minhaTabela.Columns.Add("EstacaoSaldos", Type.GetType("System.Decimal"));
            minhaTabela.Columns.Add("TipoDesconto", Type.GetType("System.Int32"));
            minhaTabela.Columns.Add("QuantidadeImpressa", Type.GetType("System.Decimal"));
            minhaTabela.Columns.Add("DescontoObrigatorio", Type.GetType("System.Int32"));
            minhaTabela.Columns.Add("ArtigoDescritivo", Type.GetType("System.Int32"));
            minhaTabela.Columns.Add("NrConsulta", Type.GetType("System.Int32"));
            minhaTabela.Columns.Add("SerieConsulta", Type.GetType("System.String"));
            minhaTabela.Columns.Add("QtdConciliada", Type.GetType("System.Decimal"));
            minhaTabela.Columns.Add("NrConsultaOrigem", Type.GetType("System.Int32"));
            minhaTabela.Columns.Add("SerieConsultaOrigem", Type.GetType("System.String"));
            minhaTabela.Columns.Add("EspacoFiscal", Type.GetType("System.String"));
            minhaTabela.Columns.Add("TipoImposto", Type.GetType("System.String"));
            minhaTabela.Columns.Add("MotivoDesconto", Type.GetType("System.Int32"));



            return minhaTabela;
        }

        private string getEmailAdministradorSite() {
            SqlConnection emailAdminCon = new SqlConnection(connection);
            string queryForAdmin = @"SELECT ISNULL(NomeVariavel, '') AS NomeVariavel,Valor FROM istk18000 WITH (NOLOCK) WHERE (NomeVariavel IN ('sEmailAdministradorSite')) 
        and web=1 ORDER BY grupo ";
            SqlCommand SqlExecuteAdmin = new SqlCommand(queryForAdmin, emailAdminCon);
            SqlDataReader sqlDRAdmin = null;
            string adminMail = string.Empty;
            try {
                sqlDRAdmin = SqlExecuteAdmin.ExecuteReader();
                if (sqlDRAdmin.Read()) {
                    adminMail = sqlDRAdmin.GetString(sqlDRAdmin.GetOrdinal("Valor"));
                }
                sqlDRAdmin.Close();
                emailAdminCon.Close();
            }
            catch {
                if(sqlDRAdmin != null) {
                    if (!sqlDRAdmin.IsClosed)
                        sqlDRAdmin.Close();
                }
                emailAdminCon.Close();
            }
            
            return adminMail;
        }

        private string getClientName (object clientCode) {
            SqlConnection clientNameCon = new SqlConnection(connection);
            string queryForName = "SELECT Nome FROM IStk0200 WITH(NOLOCK) WHERE Codigo = " + clientCode;
            SqlCommand SqlExecuteName = new SqlCommand(queryForName, clientNameCon);
            SqlDataReader sqlDRName = null;
            string name = string.Empty;
            try {
                sqlDRName = SqlExecuteName.ExecuteReader();
                if (sqlDRName.Read()) {
                    name = sqlDRName.GetString(sqlDRName.GetOrdinal("Nome"));
                }
                sqlDRName.Close();
                clientNameCon.Close();
            }
            catch {
                if (sqlDRName != null) {
                    if (!sqlDRName.IsClosed)
                        sqlDRName.Close();
                }
                clientNameCon.Close();
            }

            return name;
        }
    }
}