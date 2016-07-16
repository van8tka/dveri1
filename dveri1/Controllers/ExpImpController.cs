using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DALdv1;
using dveri1.Models;
using Domain2;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using dveri1.DopMethod;

namespace dveri1.Controllers
{
    [Authorize]
    public class ExpImpController : Controller
    {
        private DataManager dataManager;
        public ExpImpController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
     //метод exporta данных на ПК в формате xml
     [Authorize]
     [HttpGet,ValidateInput(false)]
        public ActionResult Export(string categor)
        {
            ModelExpImp model = new ModelExpImp();
            model.Category = categor;
            model.ID = true;
            return View(model);
        }

        //объявим глобальные переменные для работы с exel
        private Excel.Application excelapp;
        private Excel.Workbook excelappworkbook;
        private Excel.Workbooks excelappworkbooks;
        private Excel.Worksheet excelworksheet;

        [Authorize]
        [HttpPost]
        public ActionResult Export(ModelExpImp model)
        {
            try
            {
                IEnumerable<VhodnyeDveri> VhDvList;
               if (model.Category != "Входные двери - весь список")
                {
                  VhDvList = dataManager.VhodnyeDvRepository.GetVhodnyeDv().Where(x => x.Proizvoditel == model.Category).OrderBy(x=>x.Id);
                }
                else
                {
                   VhDvList = dataManager.VhodnyeDvRepository.GetVhodnyeDv().OrderBy(x => x.Id);
                }

                //получив данные брабатываем их 
                //создаем приложение excel
                excelapp = new Excel.Application();
                //создаем книгу
                excelappworkbook = excelapp.Workbooks.Add(Type.Missing);
                //создадим записи в книге и указываем названия столбцов в зависимости от выбранных полей характеристик товара
                excelworksheet = excelappworkbook.ActiveSheet;
                //первой поле номер товара по умолчанию, остальные если истина то добавляем иначе нет
                excelworksheet.Cells[1, 1] = "ID";
                //создадим словарь для определения имен заголовка таблицы
                Dictionary<string, bool> ListOfHeaders = new Dictionary<string, bool>();
                ListOfHeaders.Add("Название", model.Nazvanie);
                ListOfHeaders.Add("Фирма производитель", model.Proizvoditel);
                ListOfHeaders.Add("Страна", model.StranaProizv);
                ListOfHeaders.Add("Цвет", model.Cvet);
                ListOfHeaders.Add("Наполнитель", model.Napolnitel);
                ListOfHeaders.Add("Уплотнитель", model.Yplotnitel);
                ListOfHeaders.Add("Толщина металла", model.TolschinaMetala);
                ListOfHeaders.Add("Толщина дверного полотна", model.TolschinaDvPolotna);
                ListOfHeaders.Add("Фурнитура", model.Furnitura);
                ListOfHeaders.Add("Петли", model.Petli);
                ListOfHeaders.Add("Отделка снаружи", model.OtdSnarugi);
                ListOfHeaders.Add("Отделка внутри", model.OtdVnutri);
                ListOfHeaders.Add("Цена", model.Cena);
                ListOfHeaders.Add("Скидка", model.Skidka);
                ListOfHeaders.Add("Публикация", model.Publicaciya);
                ListOfHeaders.Add("Описание", model.Opisanie);
                ListOfHeaders.Add("Дополнительное описание", model.DopChar);
                //определим количество ячеек в заголовке
                int countOfCellsHead = 18;
                //определим текущее имя выбранной ячейки
                int currentNameCell = 0;
                int RemoveAllCells = 0;
                //кол-во дверей
                int countDv = VhDvList.Count();
                //заполним индексы
                for(int str=2;str<countDv+2;str++)
                {
                    excelworksheet.Cells[str, 1] = VhDvList.ElementAt(str - 2).Id.ToString();
                }

                //проход по ячейкам заголовка
                for (int currentCellHead = 2; currentCellHead <= countOfCellsHead; currentCellHead++)
                {//взяв номер ячейки заголовка, пройдем по выбрнным полям
                    
                    for (int IndexNameCell = currentNameCell; IndexNameCell <=16; IndexNameCell++)
                    {//если поле выбрано, то этой ячейке присвоим имя и выйдем из цикла
                        if (ListOfHeaders.ElementAt(IndexNameCell).Value)
                        {
                            excelworksheet.Cells[1, currentCellHead] = ListOfHeaders.ElementAt(IndexNameCell).Key;
                            //заполним столбец данными из таблицы БД,начинаем со 2-ой строки
                            if (ListOfHeaders.ElementAt(IndexNameCell).Value == true&&ListOfHeaders.ElementAt(IndexNameCell).Key== "Название")
                            {
                                for (int stroka = 2; stroka < countDv + 2; stroka++)
                                {
                                    excelworksheet.Cells[stroka, currentCellHead] = VhDvList.ElementAt(stroka - 2).Nazvanie;
                                }
                            }
                            if (ListOfHeaders.ElementAt(IndexNameCell).Value == true && ListOfHeaders.ElementAt(IndexNameCell).Key == "Фирма производитель")
                            {
                                for (int stroka = 2; stroka < countDv + 2; stroka++)
                                {
                                    excelworksheet.Cells[stroka, currentCellHead] = VhDvList.ElementAt(stroka - 2).Proizvoditel;
                                }
                            }
                            if (ListOfHeaders.ElementAt(IndexNameCell).Value == true && ListOfHeaders.ElementAt(IndexNameCell).Key == "Страна")
                            {
                                for (int stroka = 2; stroka < countDv + 2; stroka++)
                                {
                                    excelworksheet.Cells[stroka, currentCellHead] = VhDvList.ElementAt(stroka - 2).Strana;
                                }
                            }
                            if (ListOfHeaders.ElementAt(IndexNameCell).Value == true && ListOfHeaders.ElementAt(IndexNameCell).Key == "Цвет")
                            {
                                for (int stroka = 2; stroka < countDv + 2; stroka++)
                                {
                                    excelworksheet.Cells[stroka, currentCellHead] = VhDvList.ElementAt(stroka - 2).Cvet;
                                }
                            }
                            if (ListOfHeaders.ElementAt(IndexNameCell).Value == true && ListOfHeaders.ElementAt(IndexNameCell).Key == "Наполнитель")
                            {
                                for (int stroka = 2; stroka < countDv + 2; stroka++)
                                {
                                    excelworksheet.Cells[stroka, currentCellHead] = VhDvList.ElementAt(stroka - 2).Napolnitel;
                                }
                            }
                            if (ListOfHeaders.ElementAt(IndexNameCell).Value == true && ListOfHeaders.ElementAt(IndexNameCell).Key == "Уплотнитель")
                            {
                                for (int stroka = 2; stroka < countDv + 2; stroka++)
                                {
                                    excelworksheet.Cells[stroka, currentCellHead] = VhDvList.ElementAt(stroka - 2).Yplotnitel;
                                }
                            }
                            if (ListOfHeaders.ElementAt(IndexNameCell).Value == true && ListOfHeaders.ElementAt(IndexNameCell).Key == "Толщина металла")
                            {
                                for (int stroka = 2; stroka < countDv + 2; stroka++)
                                {
                                    excelworksheet.Cells[stroka, currentCellHead] = VhDvList.ElementAt(stroka - 2).TolschinaMetalla.ToString();
                                }
                            }
                            if (ListOfHeaders.ElementAt(IndexNameCell).Value == true && ListOfHeaders.ElementAt(IndexNameCell).Key == "Толщина дверного полотна")
                            {
                                for (int stroka = 2; stroka < countDv + 2; stroka++)
                                {
                                    excelworksheet.Cells[stroka, currentCellHead] = VhDvList.ElementAt(stroka - 2).TolschinaDvPolotna.ToString();
                                }
                            }
                            if (ListOfHeaders.ElementAt(IndexNameCell).Value == true && ListOfHeaders.ElementAt(IndexNameCell).Key == "Фурнитура")
                            {
                                for (int stroka = 2; stroka < countDv + 2; stroka++)
                                {
                                    excelworksheet.Cells[stroka, currentCellHead] = VhDvList.ElementAt(stroka - 2).Furnitura;
                                }
                            }
                            if (ListOfHeaders.ElementAt(IndexNameCell).Value == true && ListOfHeaders.ElementAt(IndexNameCell).Key == "Петли")
                            {
                                for (int stroka = 2; stroka < countDv + 2; stroka++)
                                {
                                    excelworksheet.Cells[stroka, currentCellHead] = VhDvList.ElementAt(stroka - 2).Petli;
                                }
                            }
                            if (ListOfHeaders.ElementAt(IndexNameCell).Value == true && ListOfHeaders.ElementAt(IndexNameCell).Key == "Отделка снаружи")
                            {
                                for (int stroka = 2; stroka < countDv + 2; stroka++)
                                {
                                    excelworksheet.Cells[stroka, currentCellHead] = VhDvList.ElementAt(stroka - 2).OtdelkaSnarugi;
                                }
                            }
                            if (ListOfHeaders.ElementAt(IndexNameCell).Value == true && ListOfHeaders.ElementAt(IndexNameCell).Key == "Отделка внутри")
                            {
                                for (int stroka = 2; stroka < countDv + 2; stroka++)
                                {
                                    excelworksheet.Cells[stroka, currentCellHead] = VhDvList.ElementAt(stroka - 2).OtdelkaVnutri;
                                }
                            }
                            if (ListOfHeaders.ElementAt(IndexNameCell).Value == true && ListOfHeaders.ElementAt(IndexNameCell).Key == "Цена")
                            {
                                for (int stroka = 2; stroka < countDv + 2; stroka++)
                                {
                                    if(VhDvList.ElementAt(stroka - 2).Cena != null)
                                    {
                                        excelworksheet.Cells[stroka, currentCellHead] = VhDvList.ElementAt(stroka - 2).Cena.Value.ToString("C");
                                    }
                                    else
                                    {
                                        excelworksheet.Cells[stroka, currentCellHead] = "Цена не установлена";
                                    }                                   
                                }
                            }
                            if (ListOfHeaders.ElementAt(IndexNameCell).Value == true && ListOfHeaders.ElementAt(IndexNameCell).Key == "Скидка")
                            {
                                for (int stroka = 2; stroka < countDv + 2; stroka++)
                                {
                                    excelworksheet.Cells[stroka, currentCellHead] = VhDvList.ElementAt(stroka - 2).Skidka.ToString();
                                }
                            }
                            if (ListOfHeaders.ElementAt(IndexNameCell).Value == true && ListOfHeaders.ElementAt(IndexNameCell).Key == "Публикация")
                            {
                                for (int stroka = 2; stroka < countDv + 2; stroka++)
                                {
                                    excelworksheet.Cells[stroka, currentCellHead] = VhDvList.ElementAt(stroka - 2).Publicaciya.ToString();
                                }
                            }
                            if (ListOfHeaders.ElementAt(IndexNameCell).Value == true && ListOfHeaders.ElementAt(IndexNameCell).Key == "Описание")
                            {
                                for (int stroka = 2; stroka < countDv + 2; stroka++)
                                {
                                    excelworksheet.Cells[stroka, currentCellHead] = VhDvList.ElementAt(stroka - 2).Opisanie;
                                }
                            }
                            if (ListOfHeaders.ElementAt(IndexNameCell).Value == true && ListOfHeaders.ElementAt(IndexNameCell).Key == "Дополнительное описание")
                            {
                                for (int stroka = 2; stroka < countDv + 2; stroka++)
                                {
                                    excelworksheet.Cells[stroka, currentCellHead] = VhDvList.ElementAt(stroka - 2).DopCharacteristics;
                                }
                            }
                            //если не равно 16 то переходим к следующему полю
                            if (IndexNameCell != 16)
                                currentNameCell = IndexNameCell + 1;
                            else
                                currentNameCell = IndexNameCell;
                            break;
                        }
                        else
                        {
                            RemoveAllCells++;
                        }
                    }
                    //перейдем к другой ячейки
                    countOfCellsHead = 18 - RemoveAllCells;
                }

//выделим первую строку с названиями столбцов
                excelworksheet.get_Range("A1", "Y1").Font.Bold = true;
                excelworksheet.get_Range("A1", "Y1").Interior.ColorIndex = 33;
                excelworksheet.get_Range("A2", "Y1").ColumnWidth = 20;
                excelworksheet.get_Range("A1", "Y65000").RowHeight = 25;


                //формат сохранения
                string pathDog = Server.MapPath("~//App_data//Excel//");
                //проверяем наличие папки
                if (!Directory.Exists(pathDog))
                    Directory.CreateDirectory(pathDog);
                excelapp.DefaultFilePath = pathDog;
                //фотмат сохранения xlExcel12 равен .xlsx
                excelapp.DefaultSaveFormat = Excel.XlFileFormat.xlExcel12;
                //запрос перезаписи книги - да перезаписать
                excelapp.DisplayAlerts = false;
                //получим список книг
                //excelappworkbooks = excelapp.Workbooks;

                //получим ссылку на первую книгу
                //excelappworkbook = excelappworkbooks[1];
                excelappworkbook.Save();

                //закрытие программы
                excelappworkbook.Close();
                excelapp.Quit();
                //полностью убиваем прилагу
                KillExcel();
               
                //вывод окна для сохранения файла
                string p = Server.MapPath("~//App_data//Excel//");
                string file = Path.Combine(p + "Книга1.xlsx");
                DirectoryInfo dir = new DirectoryInfo(p);
                string contentType = "application/xlsx";
                return File(dir + "Книга1.xlsx", contentType, "Книга1.xlsx");
            }
            catch(Exception er)
            {
                ClassLog.Write("ExpImpController/Export- " + er);
                return View("Error");
            }
         }
        //метод убийтва процесса Exel
        private void KillExcel()
        {
            System.Diagnostics.Process[] localByName = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process pr in localByName)
                if (pr.ProcessName == "EXCEL") pr.Kill();
        }
    }
  
}