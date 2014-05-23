using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Data;

namespace HotBot.GUI
{
    public class ListViewHelper
    {
        private XmlDocument xmlDocument;
        XmlElement rootNode;

        public ListViewHelper()
        {
            Clear();
        }

        public int Count = 0;

        public void Clear()
        {
            xmlDocument = new XmlDocument();
            rootNode = xmlDocument.CreateElement("ListViewDataRows");
            xmlDocument.AppendChild(rootNode);
        }

        public void AddRow(Dictionary<string, string> row)
        {
            XmlElement childNodeRow = xmlDocument.CreateElement("ListViewDataRow");

            foreach (KeyValuePair<string, string> kvp in row)
            {
                XmlElement childNodeField = xmlDocument.CreateElement(kvp.Key);
                XmlText childNodeFieldText = xmlDocument.CreateTextNode(kvp.Value);
                childNodeFieldText.Value = kvp.Value;

                childNodeRow.AppendChild(childNodeField);
                childNodeField.AppendChild(childNodeFieldText);
            }

            rootNode.AppendChild(childNodeRow);

            Count++;
        }

        public DataView GetDataContext()
        {
            return GetDataSet().Tables[0].DefaultView;
        }

        public DataSet GetDataSet()
        {
            xmlDocument.ToString();

            DataSet ds = new DataSet("Table");
            //ds.Tables.Add();
            //ds.Tables[0].Columns.Add("Sphere", typeof(double));
            ds.ReadXml(new XmlTextReader(new System.IO.MemoryStream(Encoding.ASCII.GetBytes(xmlDocument.InnerXml))));

            /*ds.Tables[0].Columns[0].DataType = typeof(int);

            // look for ints to set the datatype - it's dangerous!
            foreach (DataRowView row in ds.Tables[0].Rows)
            {
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    double result;

                    if (double.TryParse(row[i].ToString(), out result))
                    {
                        ds.Tables[0].Columns[i].DataType = typeof(double);
                    }
                }
            }*/


            return ds;
        }
    }
}
