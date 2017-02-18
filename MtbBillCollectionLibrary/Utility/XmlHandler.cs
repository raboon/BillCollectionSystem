using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Security.AccessControl;
using System.Text;
using System.Xml;
using System.Xml.Serialization;


namespace MtbBillCollectionLibrary.Utility
{
    public class XmlHandler
    {
        private XmlSerializer _xmlSerializer=null;
        System.IO.FileStream _fileStream = null;

        public void GenerateXml<T>(List<T> list, string xmlFilePath)
        {
            
            try
            {
                _xmlSerializer = new XmlSerializer(typeof(List<T>));
                _fileStream = new FileStream(xmlFilePath, FileMode.Create, FileAccess.Write);
                _xmlSerializer.Serialize(_fileStream,list);
            }catch(Exception exception)
            {
                throw exception;
            }
            finally
            {
                if (_fileStream != null)
                    _fileStream.Close();
                _xmlSerializer = null;
            }
        }

        public BufferedStream GenerateXml<T>(List<T> list)
        {
           BufferedStream bufferedStream = new BufferedStream(new MemoryStream());
            try
            {
                _xmlSerializer = new XmlSerializer(typeof(List<T>));
                _xmlSerializer.Serialize(bufferedStream, list);
                
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                _xmlSerializer = null;
            }
            return bufferedStream;
        }

        public List<T> ReadXml<T>(string xmlFilePath)
        {
            List<T> list=null;
            try
            {
                _xmlSerializer = new XmlSerializer(typeof(List<T>));
                _fileStream = new FileStream(xmlFilePath, FileMode.Open, FileAccess.Read);
                list = (List<T>)_xmlSerializer.Deserialize(_fileStream);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                if (_fileStream != null)
                    _fileStream.Close();
                _xmlSerializer = null;
            }
            return list;
        }

        public List<T> ReadXml<T>(BufferedStream bufferedStream)
        {
            List<T> list = null;
            try
            {
                _xmlSerializer = new XmlSerializer(typeof(List<T>));
                list = (List<T>)_xmlSerializer.Deserialize(bufferedStream);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                if (_fileStream != null)
                    _fileStream.Close();
                _xmlSerializer = null;
            }
            return list;
        }

        public void AddNewNode<T>(T obj, string xmlFilePath)
        {
            try
            {
                List<T> list = ReadXml<T>(xmlFilePath);
                list.Add(obj);
                GenerateXml<T>(list, xmlFilePath);

            }catch(Exception exception)
            {
                throw exception;
            }

        }

        public void RemoveNodeAt<T>(int position,string xmlFilePath)
        {
            try
            {
                List<T> list = ReadXml<T>(xmlFilePath);
                list.RemoveAt(position);
                GenerateXml<T>(list, xmlFilePath);

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

    }
}
