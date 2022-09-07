using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using OnlinePaymentsIntegration.SIBS.SDK;

namespace OnlinePaymentsIntegration.SaveToBD
{
    public class SaveForInitialTransaction
    {
        private SqlConnection sqlcon;
        private DataTable dtbasket;
        private int vendedorpordefeito, gv_resumoFinal_Rows_count;
        private string merchantTransactionId, tb_comentarios_Text;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtbasket">DataTable for the shop basket</param>
        /// <param name="vendedorpordefeito"> Utils.vendedorpordefeito</param>
        /// <param name="merchantTransactionId">Given merchant transaction ID</param>
        ///<param name="gv_resumoFinal_Rows_count">Gv_resumoFinal.Rows.Count</param>
        ///<param name="tb_comentarios_Text">Tb_comentarios_Text</param>
        public SaveForInitialTransaction(DataTable dtbasket, int vendedorpordefeito, string merchantTransactionId,
            string tb_comentarios_Text, int gv_resumoFinal_Rows_count) {
            this.dtbasket = dtbasket;
            this.vendedorpordefeito = vendedorpordefeito;
            this.merchantTransactionId = merchantTransactionId;
            this.gv_resumoFinal_Rows_count = gv_resumoFinal_Rows_count;
            this.tb_comentarios_Text = tb_comentarios_Text;
        }

        /// <summary>
        /// Construtor for tests
        /// </summary>
        /// <param name="vendedorpordefeito"></param>
        /// <param name="merchantTransactionId"></param>
        ///<param name="gv_resumoFinal_Rows_count">Gv_resumoFinal.Rows.Count</param>
        ///<param name="tb_comentarios_Text">Tb_comentarios_Text</param>
        public SaveForInitialTransaction(int vendedorpordefeito, string merchantTransactionId,
            string tb_comentarios_Text, int gv_resumoFinal_Rows_count) {
            this.vendedorpordefeito = vendedorpordefeito;
            this.merchantTransactionId = merchantTransactionId;
            this.gv_resumoFinal_Rows_count = gv_resumoFinal_Rows_count;
            this.tb_comentarios_Text = tb_comentarios_Text;
        }

        /// <summary>
        /// Inserts into Transaction Table the initial information for later be update on endpoint
        /// </summary>
        public void saveToTransactionsBD(string lb_infodataprevista_Text, string mailBasketBody) {
            try {
                sqlcon = new SqlConnection("Data Source=sql.inovanet.pt,3433;Initial Catalog=pizzarte_testes;User ID=pizzartenet;Password=qjaabsuf6969$;encrypt=true;trustServerCertificate=true");
                sqlcon.Open();
            }
            catch {
                sqlcon.Close();
            }
            try {
                string insertQuery = "INSERT INTO TRANSACTIONS (ClientId, TransactionId, Merchant_TransactionID, Amount_Value" +
                    ", Amount_Currency, Payment_Status, DataHoraEntrega, Mail_Basket_Body, Observations) VALUES ('" + TransactionDataForForm.clientId + "', " +
                    "'" + TransactionDataForForm.transactionID + "', " +
                    "'" + merchantTransactionId + "', " + TransactionDataForForm.amount + ", 'EUR', 'Waiting','" + lb_infodataprevista_Text + "', '" + mailBasketBody + "', '" + tb_comentarios_Text + "')";
                SqlCommand cmd = new SqlCommand(insertQuery, sqlcon);
                cmd.ExecuteNonQuery();
                sqlcon.Close();
            }
            catch {
                
                sqlcon.Close();
            }
        }

        /// <summary>
        /// Saves to a temporary itsk48000 the data of the basket
        /// </summary>
        /// <param name="iCaixa"></param>
        /// <param name="lbidcliente"></param>
        /// <param name="construirCodigoSincronismo">Utils.ConstruirCodigoSincronismo()</param>
        /// <param name="lb_infohoraprevista_Text">Lb_infohoraprevista.Text</param>
        /// <param name="lb_infodataprevista_Text">Lb_infodataprevista.Text</param>
        /// <param name="lb_nomecliente">Label Lb_nomecliente = (Label)Master.FindControl("Lb_Logado");</param>
        /// <param name="lb_tlmlogado">Label Lb_tlmlogado = (Label)Master.FindControl("Lb_tlmlogado");</param>
        public void saveToTempBD(int iCaixa, string lbidcliente, string construirCodigoSincronismo, string lb_infohoraprevista_Text,
             string lb_nomecliente, string lb_tlmlogado) {
            try {
                sqlcon = new SqlConnection("Data Source=sql.inovanet.pt,3433;Initial Catalog=pizzarte_testes;User ID=pizzartenet;Password=qjaabsuf6969$;encrypt=true;trustServerCertificate=true");
                sqlcon.Open();
            }
            catch {
                sqlcon.Close();
            }

            string horalevantamento = "";
            horalevantamento = lb_infohoraprevista_Text;
            

            SqlTransaction transactionlocal = sqlcon.BeginTransaction();
            SqlDataReader sqlDR = null;
            SqlCommand SqlExecute0 = null;

            try {
                //cria cabecalho, tirado da pizzarte, original da Sara

                foreach (DataRow row in dtbasket.Rows) {


                    string sSqlDetalhes = string.Empty;
                    sSqlDetalhes = "INSERT INTO IStk48000_Copy(Pizzarte_Client_Code,TransactionId, Merchant_TransactionId, Vendedor, Referencia, Quantidade, Hora, PrecoUnitarioComIva,";// Data,
                    sSqlDetalhes += "PrecoUnitario, Marcado, TaxaIva, DescontoLinha, DescontoLinhaDefeito, DescontoMaximoArtigo,";
                    sSqlDetalhes += "PrecoAgrupado, DescontoAgrupado, EstacaoSaldos, TipoDesconto, ArtigoKit, QuantidadeImpressa,";
                    sSqlDetalhes += "ArtigoPeso, CodSincr, CodLinha, composto, TemPerfis, Com, Sem, Perfil, Predefinido, DescontoObrigatorio,";
                    sSqlDetalhes += "ArtigoDescritivo, Descricao, Seccoes, NrConsulta, SerieConsulta, QtdConciliada,";
                    sSqlDetalhes += "NrConsultaOrigem, SerieConsultaOrigem, IncluirExtrasNaFactura, Armazem, Preceito)";
                    sSqlDetalhes += "VALUES('" + TransactionDataForForm.transactionID + "','" + merchantTransactionId + "'," + vendedorpordefeito + ",'" + row[1] + "'," + row[2].ToString().Replace(",", ".") + ",";
                    if (row[15].ToString().ToUpper() == "TRUE" || row[16].ToString().ToUpper() == "TRUE")
                        sSqlDetalhes += "'', ";
                    else
                        sSqlDetalhes += "FORMAT(CAST(GETDATE() AS DATETIME), 'HH:mm:ss'), ";
                    sSqlDetalhes += "" + row[3].ToString().Replace(",", ".") + ",";
                    sSqlDetalhes += "" + row[4].ToString().Replace(",", ".") + ",0," + row[6].ToString().Replace(",", ".") + "," + row[7].ToString().Replace(",", ".") + ",0," + row[8].ToString().Replace(",", ".") + ", ";
                    sSqlDetalhes += "0,0,0,0,'" + row[9] + "',0, ";
                    sSqlDetalhes += "'" + row[10] + "','" + row[11] + "','" + row[12] + "','" + row[13] + "','" + row[14] + "','" + row[15] + "','" + row[16] + "'," + row[17] + ",'" + row[18] + "',0, ";
                    sSqlDetalhes += "0,'" + row[19].ToString().Replace("'", "´") + "','" + row[20] + "',0,'',0,";
                    sSqlDetalhes += "0,'','" + row[21] + "'," + row[22] + "," + row[23] + ") ";

                    SqlExecute0 = new SqlCommand(sSqlDetalhes, sqlcon, transactionlocal);
                    sqlDR = SqlExecute0.ExecuteReader();
                    sqlDR.Close();
                }
                if (gv_resumoFinal_Rows_count != 0) {
                    //adiciona observação //SECCAO 3
                    if (tb_comentarios_Text != "") {
                        string sSqlDetalhes = string.Empty;
                        sSqlDetalhes = "INSERT INTO IStk48000_Copy(Pizzarte_Client_Code,TransactionId, Merchant_TransactionId, Vendedor, Referencia, Quantidade, Hora, PrecoUnitarioComIva,";// Data,
                        sSqlDetalhes += "PrecoUnitario, Marcado, TaxaIva, DescontoLinha, DescontoLinhaDefeito, DescontoMaximoArtigo,";
                        sSqlDetalhes += "PrecoAgrupado, DescontoAgrupado, EstacaoSaldos, TipoDesconto, ArtigoKit, QuantidadeImpressa,";
                        sSqlDetalhes += "ArtigoPeso, CodSincr, CodLinha, composto, TemPerfis, Com, Sem, Perfil, Predefinido, DescontoObrigatorio,";
                        sSqlDetalhes += "ArtigoDescritivo, Descricao, Seccoes, NrConsulta, SerieConsulta, QtdConciliada,";
                        sSqlDetalhes += "NrConsultaOrigem, SerieConsultaOrigem, IncluirExtrasNaFactura, Armazem, Preceito)";
                        sSqlDetalhes += "VALUES(" + lbidcliente + ",'" + TransactionDataForForm.transactionID + "','" + merchantTransactionId + "'," + vendedorpordefeito + ",'',0,FORMAT(CAST(GETDATE() AS DATETIME), 'HH:mm:ss'),0,";
                        sSqlDetalhes += "0,0,0,0,0,0, ";
                        sSqlDetalhes += "0,0,0,0,'" + false + "',0, ";
                        sSqlDetalhes += "'" + false + "','" + construirCodigoSincronismo + "','" + construirCodigoSincronismo + "','" + false + "','" + false + "','" + false + "','" + false + "',0,'" + false + "',0, ";
                        sSqlDetalhes += "0,'" + tb_comentarios_Text + "','3',0,'',0, ";
                        sSqlDetalhes += "0,'','" + false + "',0,0) ";
                        //sqlDR1 = Data.Consulta(sqlcon, sSqlDetalhes, true, transactionlocal);
                        SqlExecute0 = new SqlCommand(sSqlDetalhes, sqlcon, transactionlocal);
                        sqlDR = SqlExecute0.ExecuteReader();
                        sqlDR.Close();
                    }

                    //adiciona hora de levantamento //SECCAO 3
                    string sSqlDetalhes1 = string.Empty;
                    sSqlDetalhes1 = "INSERT INTO IStk48000_Copy(Pizzarte_Client_Code,TransactionId, Merchant_TransactionId, Vendedor, Referencia, Quantidade, Hora, PrecoUnitarioComIva,";// Data,
                    sSqlDetalhes1 += "PrecoUnitario, Marcado, TaxaIva, DescontoLinha, DescontoLinhaDefeito, DescontoMaximoArtigo,";
                    sSqlDetalhes1 += "PrecoAgrupado, DescontoAgrupado, EstacaoSaldos, TipoDesconto, ArtigoKit, QuantidadeImpressa,";
                    sSqlDetalhes1 += "ArtigoPeso, CodSincr, CodLinha, composto, TemPerfis, Com, Sem, Perfil, Predefinido, DescontoObrigatorio,";
                    sSqlDetalhes1 += "ArtigoDescritivo, Descricao, Seccoes, NrConsulta, SerieConsulta, QtdConciliada,";
                    sSqlDetalhes1 += "NrConsultaOrigem, SerieConsultaOrigem, IncluirExtrasNaFactura, Armazem, Preceito)";
                    sSqlDetalhes1 += "VALUES(" + lbidcliente + ",'" + TransactionDataForForm.transactionID + "','" + merchantTransactionId + "'," + vendedorpordefeito + ",'',0,FORMAT(CAST(GETDATE() AS DATETIME), 'HH:mm:ss'),0,";
                    sSqlDetalhes1 += "0,0,0,0,0,0, ";
                    sSqlDetalhes1 += "0,0,0,0,'" + false + "',0, ";
                    sSqlDetalhes1 += "'" + false + "','" + construirCodigoSincronismo + "','" + construirCodigoSincronismo + "','" + false + "','" + false + "','" + false + "','" + false + "',0,'" + false + "',0, ";
                    sSqlDetalhes1 += "0,'Levantamento às: " + horalevantamento + "','3',0,'',0, ";
                    sSqlDetalhes1 += "0,'','" + false + "',0,0) ";
                    SqlExecute0 = new SqlCommand(sSqlDetalhes1, sqlcon, transactionlocal);
                    sqlDR = SqlExecute0.ExecuteReader();
                    sqlDR.Close();
                    //}
                    
                    string sSqlDetalhes2 = string.Empty;
                    sSqlDetalhes2 = "INSERT INTO IStk48000_Copy(Pizzarte_Client_Code,TransactionId, Merchant_TransactionId, Vendedor, Referencia, Quantidade, Hora, PrecoUnitarioComIva,";// Data,
                    sSqlDetalhes2 += "PrecoUnitario, Marcado, TaxaIva, DescontoLinha, DescontoLinhaDefeito, DescontoMaximoArtigo,";
                    sSqlDetalhes2 += "PrecoAgrupado, DescontoAgrupado, EstacaoSaldos, TipoDesconto, ArtigoKit, QuantidadeImpressa,";
                    sSqlDetalhes2 += "ArtigoPeso, CodSincr, CodLinha, composto, TemPerfis, Com, Sem, Perfil, Predefinido, DescontoObrigatorio,";
                    sSqlDetalhes2 += "ArtigoDescritivo, Descricao, Seccoes, NrConsulta, SerieConsulta, QtdConciliada,";
                    sSqlDetalhes2 += "NrConsultaOrigem, SerieConsultaOrigem, IncluirExtrasNaFactura, Armazem, Preceito)";
                    sSqlDetalhes2 += "VALUES("+ lbidcliente +",'" + TransactionDataForForm.transactionID + "','" + merchantTransactionId + "'," + vendedorpordefeito + ",'',0,FORMAT(CAST(GETDATE() AS DATETIME), 'HH:mm:ss'),0,";
                    sSqlDetalhes2 += "0,0,0,0,0,0, ";
                    sSqlDetalhes2 += "0,0,0,0,'" + false + "',0, ";
                    sSqlDetalhes2 += "'" + false + "','" + construirCodigoSincronismo + "','" + construirCodigoSincronismo + "','" + false + "','" + false + "','" + false + "','" + false + "',0,'" + false + "',0, ";
                    sSqlDetalhes2 += "0,'Nº" + lbidcliente + "-" + lb_nomecliente + "','0',0,'',0, ";
                    sSqlDetalhes2 += "0,'','" + false + "',0,0) ";
                    SqlExecute0 = new SqlCommand(sSqlDetalhes2, sqlcon, transactionlocal);
                    sqlDR = SqlExecute0.ExecuteReader();
                    sqlDR.Close();


                    string sSqlDetalhes3 = string.Empty;
                    sSqlDetalhes3 = "INSERT INTO IStk48000_Copy(Pizzarte_Client_Code,TransactionId, Merchant_TransactionId, Vendedor, Referencia, Quantidade, Hora, PrecoUnitarioComIva,";// Data,
                    sSqlDetalhes3 += "PrecoUnitario, Marcado, TaxaIva, DescontoLinha, DescontoLinhaDefeito, DescontoMaximoArtigo,";
                    sSqlDetalhes3 += "PrecoAgrupado, DescontoAgrupado, EstacaoSaldos, TipoDesconto, ArtigoKit, QuantidadeImpressa,";
                    sSqlDetalhes3 += "ArtigoPeso, CodSincr, CodLinha, composto, TemPerfis, Com, Sem, Perfil, Predefinido, DescontoObrigatorio,";
                    sSqlDetalhes3 += "ArtigoDescritivo, Descricao, Seccoes, NrConsulta, SerieConsulta, QtdConciliada,";
                    sSqlDetalhes3 += "NrConsultaOrigem, SerieConsultaOrigem, IncluirExtrasNaFactura, Armazem, Preceito)";
                    sSqlDetalhes3 += "VALUES(" + lbidcliente + ",'" + TransactionDataForForm.transactionID + "','" + merchantTransactionId + "'," + vendedorpordefeito + ",'',0,FORMAT(CAST(GETDATE() AS DATETIME), 'HH:mm:ss'),0,";
                    sSqlDetalhes3 += "0,0,0,0,0,0, ";
                    sSqlDetalhes3 += "0,0,0,0,'" + false + "',0, ";
                    sSqlDetalhes3 += "'" + false + "','" + construirCodigoSincronismo + "','" + construirCodigoSincronismo + "','" + false + "','" + false + "','" + false + "','" + false + "',0,'" + false + "',0, ";
                    sSqlDetalhes3 += "0,'Tlm:" + lb_tlmlogado + "','0',0,'',0, ";
                    sSqlDetalhes3 += "0,'','" + false + "',0,0) ";
                    SqlExecute0 = new SqlCommand(sSqlDetalhes3, sqlcon, transactionlocal);
                    sqlDR = SqlExecute0.ExecuteReader();
                    sqlDR.Close();
                }

                if ((transactionlocal != null)) 
                    transactionlocal.Commit();
                sqlcon.Close();
                }
            catch {
                if (!sqlDR.IsClosed)
                    sqlDR.Close();
                transactionlocal.Rollback();
                sqlcon.Close();
            }
        }



        public void saveToTempBDTest() {
            try {
                sqlcon = new SqlConnection("Data Source=sql.inovanet.pt,3433;Initial Catalog=pizzarte_testes;User ID=pizzartenet;Password=qjaabsuf6969$;encrypt=true;trustServerCertificate=true");
                sqlcon.Open();
            }
            catch {
                sqlcon.Close();
            }

            /*string horalevantamento = "";
            horalevantamento = lb_infohoraprevista_Text;
            string datalevantamento = "";
            datalevantamento = lb_infodataprevista_Text;*/

            SqlTransaction transactionlocal = sqlcon.BeginTransaction();
            SqlDataReader sqlDR = null;
            SqlCommand SqlExecute0 = null;

            try {
                //cria cabecalho, tirado da pizzarte, original da Sara

                


                    string sSqlDetalhes = string.Empty;
                    sSqlDetalhes = "INSERT INTO IStk48000_Copy(Pizzarte_Client_Code,TransactionId, Merchant_TransactionId, Vendedor, Referencia, Quantidade, Hora, PrecoUnitarioComIva,PrecoUnitario, Marcado, TaxaIva, DescontoLinha, DescontoLinhaDefeito, DescontoMaximoArtigo,PrecoAgrupado, DescontoAgrupado, EstacaoSaldos, TipoDesconto, ArtigoKit, QuantidadeImpressa,ArtigoPeso, CodSincr, CodLinha, composto, TemPerfis, Com, Sem, Perfil, Predefinido, DescontoObrigatorio,ArtigoDescritivo, Descricao, Seccoes, NrConsulta, SerieConsulta, QtdConciliada,NrConsultaOrigem, SerieConsultaOrigem, IncluirExtrasNaFactura, Armazem, Preceito)VALUES(2679,'" + TransactionDataForForm.transactionID+"','"+merchantTransactionId+"',1,'0101004',1,FORMAT(CAST(GETDATE() AS DATETIME), 'HH:mm:ss'), 2.50,2.21239,0,13.0000000,0,0,100.0000000, 0,0,0,0,'False',0, 'False','010  20220905143150714','010  20220905143150714','False','True','False','False',0,'False',0, 0,'PAO D´ALHO','3,5',0,'',0,0,'','False',0,0)";

                    SqlExecute0 = new SqlCommand(sSqlDetalhes, sqlcon, transactionlocal);
                    sqlDR = SqlExecute0.ExecuteReader();
                    sqlDR.Close();
                
                if (gv_resumoFinal_Rows_count != 0) {
                    //adiciona observação //SECCAO 3
                    /*if (tb_comentarios_Text != "") {
                        string sSqlDetalhes = string.Empty;
                        sSqlDetalhes = "INSERT INTO IStk48000_Copy(TransactionId, Merchant_TransactionId, Vendedor, Referencia, Quantidade, Hora, PrecoUnitarioComIva,";// Data,
                        sSqlDetalhes += "PrecoUnitario, Marcado, TaxaIva, DescontoLinha, DescontoLinhaDefeito, DescontoMaximoArtigo,";
                        sSqlDetalhes += "PrecoAgrupado, DescontoAgrupado, EstacaoSaldos, TipoDesconto, ArtigoKit, QuantidadeImpressa,";
                        sSqlDetalhes += "ArtigoPeso, CodSincr, CodLinha, composto, TemPerfis, Com, Sem, Perfil, Predefinido, DescontoObrigatorio,";
                        sSqlDetalhes += "ArtigoDescritivo, Descricao, Seccoes, NrConsulta, SerieConsulta, QtdConciliada,";
                        sSqlDetalhes += "NrConsultaOrigem, SerieConsultaOrigem, IncluirExtrasNaFactura, Armazem, Preceito)";
                        sSqlDetalhes += "VALUES('" + TransactionDataForForm.transactionID + "','" + merchantTransactionId + "'," + vendedorpordefeito + ",'',0,FORMAT(CAST(GETDATE() AS DATETIME), 'HH:mm:ss'),0,";
                        sSqlDetalhes += "0,0,0,0,0,0, ";
                        sSqlDetalhes += "0,0,0,0,'" + false + "',0, ";
                        sSqlDetalhes += "'" + false + "','" + construirCodigoSincronismo + "','" + construirCodigoSincronismo + "','" + false + "','" + false + "','" + false + "','" + false + "',0,'" + false + "',0, ";
                        sSqlDetalhes += "0,'" + tb_comentarios_Text + "','3',0,'',0, ";
                        sSqlDetalhes += "0,'','" + false + "',0,0) ";
                        //sqlDR1 = Data.Consulta(sqlcon, sSqlDetalhes, true, transactionlocal);
                        SqlExecute0 = new SqlCommand(sSqlDetalhes, sqlcon, transactionlocal);
                        sqlDR = SqlExecute0.ExecuteReader();
                        sqlDR.Close();
                    }*/

                    //adiciona hora de levantamento //SECCAO 3
                    string sSqlDetalhes1 = string.Empty;
                    sSqlDetalhes1 = @"INSERT INTO IStk48000_Copy(Pizzarte_Client_Code,TransactionId, Merchant_TransactionId, Vendedor, Referencia, Quantidade, Hora, PrecoUnitarioComIva,PrecoUnitario, Marcado, TaxaIva, DescontoLinha, DescontoLinhaDefeito, DescontoMaximoArtigo,PrecoAgrupado, DescontoAgrupado, EstacaoSaldos, TipoDesconto, ArtigoKit, QuantidadeImpressa,ArtigoPeso, CodSincr, CodLinha, composto, TemPerfis, Com, Sem, Perfil, Predefinido, DescontoObrigatorio,ArtigoDescritivo, Descricao, Seccoes, NrConsulta, SerieConsulta, QtdConciliada,NrConsultaOrigem, SerieConsultaOrigem, IncluirExtrasNaFactura, Armazem, Preceito)VALUES(2679,'" + TransactionDataForForm.transactionID + "','" + merchantTransactionId + "',1,'',0,FORMAT(CAST(GETDATE() AS DATETIME), 'HH:mm:ss'),0,0,0,0,0,0,0, 0,0,0,0,'False',0, 'False','010  20220905143253833','010  20220905143253855','False','False','False','False',0,'False',0, 0,'Levantamento às: 15:03','3',0,'',0, 0,'','False',0,0)";
                    SqlExecute0 = new SqlCommand(sSqlDetalhes1, sqlcon, transactionlocal);
                    sqlDR = SqlExecute0.ExecuteReader();
                    sqlDR.Close();
                    //}

                    string sSqlDetalhes2 = string.Empty;
                    sSqlDetalhes2 = "INSERT INTO IStk48000_Copy(Pizzarte_Client_Code,TransactionId, Merchant_TransactionId, Vendedor, Referencia, Quantidade, Hora, PrecoUnitarioComIva,PrecoUnitario, Marcado, TaxaIva, DescontoLinha, DescontoLinhaDefeito, DescontoMaximoArtigo,PrecoAgrupado, DescontoAgrupado, EstacaoSaldos, TipoDesconto, ArtigoKit, QuantidadeImpressa,ArtigoPeso, CodSincr, CodLinha, composto, TemPerfis, Com, Sem, Perfil, Predefinido, DescontoObrigatorio,ArtigoDescritivo, Descricao, Seccoes, NrConsulta, SerieConsulta, QtdConciliada,NrConsultaOrigem, SerieConsultaOrigem, IncluirExtrasNaFactura, Armazem, Preceito)VALUES(2679,'" + TransactionDataForForm.transactionID + "','" + merchantTransactionId + "',1,'',0,FORMAT(CAST(GETDATE() AS DATETIME), 'HH:mm:ss'),0,0,0,0,0,0,0, 0,0,0,0,'False',0, 'False','010  20220905143343241','010  20220905143343258','False','False','False','False',0,'False',0, 0,'Nº2679- software','0',0,'',0, 0,'','False',0,0)";// Data,
                    SqlExecute0 = new SqlCommand(sSqlDetalhes2, sqlcon, transactionlocal);
                    sqlDR = SqlExecute0.ExecuteReader();
                    sqlDR.Close();


                    string sSqlDetalhes3 = string.Empty;
                    sSqlDetalhes3 = @"INSERT INTO IStk48000_Copy(Pizzarte_Client_Code,TransactionId, Merchant_TransactionId, Vendedor, Referencia, Quantidade, Hora, PrecoUnitarioComIva,PrecoUnitario, Marcado, TaxaIva, DescontoLinha, DescontoLinhaDefeito, DescontoMaximoArtigo,PrecoAgrupado, DescontoAgrupado, EstacaoSaldos, TipoDesconto, ArtigoKit, QuantidadeImpressa,ArtigoPeso, CodSincr, CodLinha, composto, TemPerfis, Com, Sem, Perfil, Predefinido, DescontoObrigatorio,ArtigoDescritivo, Descricao, Seccoes, NrConsulta, SerieConsulta, QtdConciliada,NrConsultaOrigem, SerieConsultaOrigem, IncluirExtrasNaFactura, Armazem, Preceito)VALUES(2679,'" + TransactionDataForForm.transactionID + "','" + merchantTransactionId + "',1,'',0,FORMAT(CAST(GETDATE() AS DATETIME), 'HH:mm:ss'),0,0,0,0,0,0,0, 0,0,0,0,'False',0, 'False','010  20220905143408647','010  20220905143408661','False','False','False','False',0,'False',0, 0,'Tlm:917826580','0',0,'',0, 0,'','False',0,0)";// Data,
                    SqlExecute0 = new SqlCommand(sSqlDetalhes3, sqlcon, transactionlocal);
                    sqlDR = SqlExecute0.ExecuteReader();
                    sqlDR.Close();
                }

                if ((transactionlocal != null))
                    transactionlocal.Commit();
                sqlcon.Close();
            }
            catch {
                if(sqlDR != null)
                    if (!sqlDR.IsClosed)
                        sqlDR.Close();
                transactionlocal.Rollback();
                sqlcon.Close();
            }
        }
    }
}