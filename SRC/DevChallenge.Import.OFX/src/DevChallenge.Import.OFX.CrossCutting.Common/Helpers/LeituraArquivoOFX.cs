using DevChallenge.Import.OFX.Api.Domain.DadosBanco.Entities;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DevChallenge.Import.OFX.CrossCutting.Common.Helpers
{
    public class LeituraArquivoOFX
    {
        /// <summary>
        /// 
        /// OFX data reading method
        /// </summary>
        /// <param name="arquivoOFX">Path of OFX source file</param>
        public static RegistroBanco ExecutarLeitura(String arquivoOFX)
        {

            var arquivoXml = $"{arquivoOFX.Replace(".ofx", string.Empty)}.xml";
            OfxToXml(arquivoOFX, arquivoXml);

            String leituraDados = "";
            DetalhesTransacao detalhesTransacao = null;
            RegistroBanco detalhes = new RegistroBanco();

            XmlTextReader xmlTransformado = new XmlTextReader (arquivoXml);

            try
            {
                while (xmlTransformado.Read())
                {
                    if (xmlTransformado.NodeType == XmlNodeType.EndElement)
                    {
                        switch (xmlTransformado.Name)
                        {
                            case "STMTTRN":
                                if (detalhesTransacao != null)
                                {
                                    detalhes.AdicionarTransacao(detalhesTransacao);
                                    detalhesTransacao = null;
                                }
                                break;
                        }
                    }
                    if (xmlTransformado.NodeType == XmlNodeType.Element)
                    {
                        leituraDados = xmlTransformado.Name;

                        switch (leituraDados)
                        {
                            case "STMTTRN":
                                detalhesTransacao = new DetalhesTransacao();
                                break;
                        }
                    }
                    if (xmlTransformado.NodeType == XmlNodeType.Text)
                    {
                        switch (leituraDados)
                        {
                            case "CODE":
                                detalhes.Code = xmlTransformado.Value;
                                break;
                            case "SEVERITY":
                                detalhes.Severity = xmlTransformado.Value;
                                break;
                            case "DTSERVER":
                                detalhes.Dtserver = ConversaoData(xmlTransformado.Value, detalhes);
                                break;
                            case "LANGUAGE":
                                detalhes.Language = xmlTransformado.Value;
                                break;
                            case "TRNUID":
                                detalhes.Trnuid = xmlTransformado.Value;
                                break;
                            case "CURDEF":
                                detalhes.Curdef = xmlTransformado.Value;
                                break;
                            case "BANKID":
                                detalhes.Bankid = ConversaoInt(xmlTransformado.Value, detalhes);
                                break;
                            case "ACCTID":
                                detalhes.Acctid = xmlTransformado.Value;
                                break;
                            case "ACCTTYPE":
                                detalhes.Accttype = xmlTransformado.Value;
                                break;
                            case "DTSTART":
                                detalhes.Dtstart = ConversaoData(xmlTransformado.Value, detalhes);
                                break;
                            case "DTEND":
                                detalhes.Dtend = ConversaoData(xmlTransformado.Value, detalhes);
                                break;
                            case "TRNTYPE":
                                detalhesTransacao.Trntype = xmlTransformado.Value;
                                break;
                            case "DTPOSTED":
                                detalhesTransacao.Dtposted = ConversaoData(xmlTransformado.Value, detalhes);
                                break;
                            case "TRNAMT":
                                detalhesTransacao.Trnamt = ConversaoDouble(xmlTransformado.Value, detalhes);
                                break;
                            case "MEMO":
                                detalhesTransacao.Memo = string.IsNullOrEmpty(xmlTransformado.Value) ? "" : xmlTransformado.Value.Trim().Replace("  ", " ");
                                break;
                        }
                    }
                }
            }
            catch (XmlException ex)
            {
                throw new FileNotFoundException("Arquivo OFX inválido : " + arquivoOFX + " " + ex.Message);
            }
            finally
            {
                xmlTransformado.Close();
            }

            return detalhes;
        }

        /// <summary>
        /// TOFX to XML conversion method.
        /// </summary>
        /// <param name="arquivoOFX">Path of OFX source file</param>
        /// <param name="ofxConvertido">Path of the XML file, internally generated.</param>
        private static void OfxToXml(String arquivoOFX, String ofxConvertido)
        {
            if (System.IO.File.Exists(arquivoOFX))
            {
                if (ofxConvertido.ToLower().EndsWith(".xml"))
                {
                    StringBuilder ofxTranslated = TranslateToXml(arquivoOFX);

                    if (System.IO.File.Exists(ofxConvertido))
                    {
                        System.IO.File.Delete(ofxConvertido);
                    }

                    StreamWriter sw = File.CreateText(ofxConvertido);
                    sw.WriteLine(@"<?xml version=""1.0""?>");
                    sw.WriteLine(ofxTranslated.ToString());
                    sw.Close();
                }
                else
                {
                    throw new ArgumentException("Nome do novo XML inválido: " + ofxConvertido);
                }
            }
            else
            {
                throw new FileNotFoundException("Arquivo OFX não encontrado: " + arquivoOFX);
            }
        }

        /// <summary>
        /// Method for reading OFX files
        /// </summary>
        /// <param name="arquivoOFX">OFX source file</param>
        /// <returns>XML tags in StringBuilder object.</returns>
        private static StringBuilder TranslateToXml(String arquivoOFX)
        {
            StringBuilder leituraLinha = new StringBuilder();
            int linha = 0;
            String tag;

            if (!File.Exists(arquivoOFX))
            {
                throw new FileNotFoundException("Arquivo OFX não foi encontrado: " + arquivoOFX);
            }

            StreamReader sr = File.OpenText(arquivoOFX);

            while ((tag = sr.ReadLine()) != null)
            {
                tag = tag.Trim();

                /// Reading opening tags
                if (tag.StartsWith("<") && tag.EndsWith(">"))
                {
                    linha++;
                    AddTabs(leituraLinha, linha, true);
                    leituraLinha.Append(tag);
                }
                /// Reading close tags
                else if (tag.StartsWith("</") && tag.EndsWith(">"))
                {
                    AddTabs(leituraLinha, linha, true);
                    linha--;
                    leituraLinha.Append(tag);
                }
                /// Reading tags with content
                else if (tag.StartsWith("<") && !tag.EndsWith(">"))
                {
                    AddTabs(leituraLinha, linha + 1, true);
                    leituraLinha.Append(tag);
                    leituraLinha.Append(RetornandoTag(tag));
                }
            }
            sr.Close();

            return leituraLinha;
        }

        /// <summary>
        /// This method return the correct closing tag string 
        /// </summary>
        /// <param name="content">Content of analysis</param>
        /// <returns>String with ending tag.</returns>
        private static String RetornandoTag(String content)
        {
            String tagFinal = "";

            if ((content.IndexOf("<") != -1) && (content.IndexOf(">") != -1))
            {
                int position1 = content.IndexOf("<");
                int position2 = content.IndexOf(">");
                if ((position2 - position1) > 2)
                {
                    tagFinal = content.Substring(position1, (position2 - position1) + 1);
                    tagFinal = tagFinal.Replace("<", "</");
                }
            }

            return tagFinal;
        }

        /// <summary>
        /// This method add tabs into lines of xml file, to best identation.
        /// </summary>
        /// <param name="stringObject">Line of content</param>
        /// <param name="lengthTabs">Length os tabs to add into content</param>
        /// <param name="newLine">Is it new line?</param>
        private static void AddTabs(StringBuilder stringObject, int lengthTabs, bool newLine)
        {
            if (newLine)
            {
                stringObject.AppendLine();
            }
            for (int j = 1; j < lengthTabs; j++)
            {
                stringObject.Append("\t");
            }
        }

        /// <summary>
        /// Date conversion method
        /// </summary>
        /// <param name="dataConversao"></param>
        /// <param name="detalhes"></param>
        /// <returns></returns>
        private static DateTime ConversaoData(String dataConversao, RegistroBanco detalhes)
        {
            DateTime dataConvertida = DateTime.MinValue;

            if (dataConversao.Length == 23)
            {
                try
                {
                    dataConvertida = new DateTime(Int32.Parse(dataConversao.Substring(0, 4))
                                                 , Int32.Parse(dataConversao.Substring(4, 2))
                                                 , Int32.Parse(dataConversao.Substring(6, 2))
                                                 , Int32.Parse(dataConversao.Substring(8, 2))
                                                 , Int32.Parse(dataConversao.Substring(10, 2))
                                                 , Int32.Parse(dataConversao.Substring(12, 2))
                                                 );
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Data inválida: " + dataConversao + " " + ex.Message);
                }
            }

            return dataConvertida;
        }

        /// <summary>
        /// Double conversion method
        /// </summary>
        /// <param name="valorConversao"></param>
        /// <param name="detalhes"></param>
        /// <returns></returns>
        private static double ConversaoDouble(string valorConversao, RegistroBanco detalhes)
        {
            double valorConvertido = 0;
            try
            {
                valorConvertido = Convert.ToDouble(valorConversao.Replace('.', ','));
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Valor inválido: " + valorConversao + " " + ex.Message);
            }
            return valorConvertido;
        }

        /// <summary>
        /// Int conversion method
        /// </summary>
        /// <param name="valorConversao"></param>
        /// <param name="detalhes"></param>
        /// <returns></returns>
        private static int ConversaoInt(string valorConversao, RegistroBanco detalhes)
        {
            int valorConvertido = 0;
            try
            {
                valorConvertido = Convert.ToInt32(valorConversao.Replace('.', ','));
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Valor inválido: " + valorConversao + " " + ex.Message);
            }
            return valorConvertido;
        }
    }
}