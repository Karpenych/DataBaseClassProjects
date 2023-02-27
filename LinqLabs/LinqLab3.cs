using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqLabs
{
    internal class LinqLab3
    {
        


        public static void StartLab3(string filePath)
        {
            XDocument doc = XDocument.Load(filePath);
            XElement? root = doc.Root;

            //-------------------------ВЫВОД ВСЕГО XML ДОКУМЕНТА-----------------------------------------//
            Console.ForegroundColor= ConsoleColor.Yellow;
            Console.WriteLine("XML файл ({0}) :\n", filePath);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("{0}\n\n", root);

            //-------------------------ДОБАВЛЕНИЕ ЭЛЕМЕНТА В ROOT-----------------------------------------//
            AddElementInRoot(root);

            //-------------------------ДОБАВЛЕНИЕ ЭЛЕМЕНТА НЕ В ROOT-----------------------------------------//
            XElement? toAddElem = root?.Element("fantasy")?.Element("book");
            AddElementInNotRootElement(toAddElem);

            //-------------------------ВЫВОД ВСЕГО XML ДОКУМЕНТА-----------------------------------------//
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Изменённый XML файл:\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("{0}\n\n", root);

            //-------------------------ВЫБОР ТЕКСТА ИЗ ЭЛЕМЕНТОВ-----------------------------------------//
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Текст из элементов:\n");
            Console.ForegroundColor = ConsoleColor.White;
            ChooseTextFromElement(root);
            Console.WriteLine();

            //-------------------------КОЛИЧЕСТВО ЭЛЕМЕНТОВ БЕЗ ДОЧЕРНИХ-----------------------------------------//
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Количество элементов не имеющих дочерних: ");
            int withoutChilderElementsCounter = 0;
            DefineElementsWithoutChildren(root, ref withoutChilderElementsCounter);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(withoutChilderElementsCounter);
            Console.WriteLine();

            //-------------------------КОЛИЧЕСТВО ЭЛЕМЕНТОВ С БОЛЕЕ 2умя ДОЧЕРНИМИ-----------------------------------------//
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Количество элементов, имеющих более двух дочерних: ");
            int withTwoMoreChilderElementsCounter = 0;
            DefineElementsWithWithTwoMoreElements(root, ref withTwoMoreChilderElementsCounter);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(withTwoMoreChilderElementsCounter);
            Console.WriteLine();
        }

        static void AddElementInRoot(XElement? root)
        {
            XElement _toAddInRoot = new
            (
                "Horror___add_In_Root",
                new XElement
                (
                    "book",
                    new XAttribute("author", "King"),
                    new XElement("name", "Pet Sematary"),
                    new XElement("price", "35")
                )
            );

            root.Add(_toAddInRoot);
        }

        static void AddElementInNotRootElement(XElement? toAddElement)
        {
            XElement _elemToAdd = new
            (
                "Add",
                new XAttribute("element", "not"),
                new XAttribute("in", "root")
            );

            toAddElement?.Add(_elemToAdd);
        }

        static void ChooseTextFromElement(XElement root)
        {
            foreach (var element in root.Elements())
            {
                 Console.WriteLine("Element name = {0}  -  Element text = {1}\n", element.Name, element.Value);
            }
        }

        static void DefineElementsWithoutChildren(XElement root, ref int ctr)
        {
            foreach (var element in root.Elements())
            {
                if (!element.Elements().Any())
                {
                    ctr++;
                }
                else
                {
                    DefineElementsWithoutChildren(element, ref ctr);
                }
            }
            return;
        }

        static void DefineElementsWithWithTwoMoreElements(XElement root, ref int ctr)
        {
            foreach (var element in root.Elements())
            {
                if (element.Elements().Count() > 2)
                {
                    ctr++;
                }
                if (element.Elements().Any())
                {
                    DefineElementsWithWithTwoMoreElements(element, ref ctr);
                }
            }
            return;
        }
    }
}
