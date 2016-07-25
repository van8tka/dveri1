﻿using System;
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
        //и глобальные константы
        private const string nazv = "Название";
        private const string proizvoditel  = "Фирма производитель";
        private const string stranaProizv ="Страна";
        private const string cvet = "Цвет";
        private const string napolnitel = "Наполнитель";
        private const string yplotnitel = "Уплотнитель";
        private const string tolschinaMetala = "Толщина металла";
        private const string tolschinaDvPolotna = "Толщина дверного полотна";
        private const string furnitura = "Фурнитура";
        private const string petl = "Петли";
        private const string otdSnarug = "Отделка снаружи";
        private const string otdVnutri = "Отделка внутри";
        private const string cena = "Цена";
        private const string skidka = "Скидка";
        private const string publicaciya = "Публикация";
        private const string opisanie = "Описание";
        private const string dopChar = "Дополнительное описание";

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
                TempData["message"] = "Данные экспортированы в файл!";
                return File(dir + "Книга1.xlsx", contentType, "каталог.xlsx");
                //RedirectToAction("Panel", "Admin");
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
        //импорт файла в БД
        [HttpGet]
        [Authorize]
        public ActionResult Import()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Import(IEnumerable<HttpPostedFileBase> fileupload)
        {
            try
            {
                //загружаем книгу на сервер

                string domainpath = Server.MapPath("~//App_Data//ForDB//");
                if (!Directory.Exists(domainpath))
                    Directory.CreateDirectory(domainpath); // Создаем директорию, если нужно
                string path = null;  
                foreach(var file in fileupload)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(file.FileName);
                        path = Path.Combine(domainpath, fileName);
                        file.SaveAs(path);
                    }
                }          
                   int flagerr=0;
                    if (path != null)
                    {
                        if (ImportToDataBase(path,ref flagerr))
                        {
                            TempData["message"] = "Данные из файла импортированы в базу данных";
                            DellAllFiles();
                            return RedirectToAction("Panel","Admin",null);
                        }
                        else
                        {    //проверка наличия ошибки в файле на наличие полей                      
                            if(flagerr==1)
                            {
                                TempData["message"] = "В импортируемом файле недостаточно данных. Должны присутствовать поля: ID, Название, Цена.";
                            }
                            else
                            {
                                TempData["message"] = "Во время импорта данных из файла произошла ошибка!!!";
                            }
                        KillExcel();
                        DellAllFiles();
                            return RedirectToAction("Panel", "Admin", null);
                        }
                    }
                    else
                    {
                    DellAllFiles();
                    return View("Error");
                    }
            }
            catch(Exception er)
            {
                KillExcel();
                DellAllFiles();
                ClassLog.Write("ExpImpController/Import- " + er);
                return RedirectToAction("Error");
            }
        }
        //метод считывания и загрузки инфы из файла excel в бд
        private bool ImportToDataBase(string pathfile,ref int flagerr)
        {
            try
            {
                excelapp = new Excel.Application();
                excelappworkbooks = excelapp.Workbooks;
                //Открываем книгу и получаем на нее ссылку
                excelappworkbook = excelapp.Workbooks.Open(pathfile, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                excelworksheet = excelappworkbook.ActiveSheet;
                string stroka;
                //кол во строк
                int str = 2;
                int AllCountHeadCell = 18;
                //переменные для сохранения номера столбца
                int iproiz=0, istrproiz = 0, ifur = 0, iotdvn = 0, iotdsnar = 0, iopis = 0, idopch = 0, itolmet = 0, itoldvpol = 0, icvet = 0, inapolnt = 0, iyplotn = 0, inazv = 0, ipubl = 0, icena = 0, iskid = 0, ipetl = 0;

                ModelExpImp model = new ModelExpImp();
                //проверка наличия полей в файле для добавления(изменения) данных
                for(int i=1;i<=AllCountHeadCell;i++)
                {
                    string j=null;
                    //проверка наличия заголовка
                    if (excelworksheet.Cells[1,i].Value!=null)
                    {//если заголовок есть то получим его значение
                       j = excelworksheet.Cells[1, i].Value2.ToString();
                    }
                    else
                    {//для проскакивания оставшихся элементов заголовка
                        i = 19;
                        if(!model.Nazvanie||!model.ID)
                        {
                              //TempData["message"] = "В импортируемом файле недостаточно данных. Должны присутствовать поля: ID, Название, Цена.";
                            flagerr = 1;
                              return false;
                        }                          
                    }
                    //получим значение ячейки в заголовке
                  
                    switch (j)
                    {//если значение совпадает то отмечаем флаг
                        case "ID":
                                {
                                model.ID = true;
                                break;
                            }
                        case nazv:
                            {
                                model.Nazvanie = true;
                                inazv = i;
                                break;
                            }
                        case proizvoditel:
                            {
                                model.Proizvoditel = true;
                                iproiz = i;
                                break;
                            }
                        case stranaProizv:
                            {
                                model.StranaProizv = true;
                                istrproiz = i;
                                break;
                            }
                        case cvet:
                            {
                                model.Cvet = true;
                                icvet = i;
                                break;
                            }
                        case napolnitel:
                            {
                                model.Napolnitel = true;
                                inapolnt = i;
                                break;
                            }
                        case yplotnitel:
                            {
                                model.Yplotnitel = true;
                                iyplotn = i;
                                break;
                            }
                        case tolschinaMetala:
                            {
                                model.TolschinaMetala = true;
                                itolmet = i;
                                break;
                            }
                        case tolschinaDvPolotna:
                            {
                                model.TolschinaDvPolotna = true;
                                itoldvpol = i;
                                break;
                            }
                        case cena:
                            {
                                model.Cena = true;
                                icena = i;
                                break;
                            }
                        case publicaciya:
                            {
                                model.Publicaciya = true;
                                ipubl = i;
                                break;
                            }
                        case skidka:
                            {
                                model.Skidka = true;
                                iskid = i;
                                break;
                            }
                        case petl:
                            {
                                model.Petli = true;
                                ipetl = i;
                                break;
                            }
                        case otdSnarug:
                            {
                                model.OtdSnarugi = true;
                                iotdsnar = i;
                                break;
                            }
                        case otdVnutri:
                            {
                                model.OtdVnutri= true;
                                iotdvn = i;
                                break;
                            }
                        case opisanie:
                            {
                                model.Opisanie = true;
                                iopis = i;
                                break;
                            }
                        case dopChar:
                            {
                                model.DopChar = true;
                                idopch = i;
                                break;
                            }
                        case furnitura:
                            {
                                model.Furnitura = true;
                                ifur = i;
                                break;
                            }
                        default:
                            break;
                    }
                }


               if(excelworksheet.Cells[str, 1].Value == null)
                {
                    return false;
                }

                //проходим по строкам пока есть ID
                while (excelworksheet.Cells[str, 1].Value != null)
                {     
                    CreateVhMod m = new CreateVhMod();
                    decimal? CenaSoSkidkoy = null;
                    //получаем значение ID
                   int ID = int.Parse(excelworksheet.Cells[str, 1].Value2.ToString());
                    //проверим наличие элемента в БД
                    VhodnyeDveri vh = new VhodnyeDveri();
                    vh = dataManager.VhodnyeDvRepository.GetVhodnyeDv().Where(x => x.Id == ID).FirstOrDefault();
                    if (vh == null)
                    {
                        ID = 0;
                    }
                    //если есть название в заголовках таблицы файла
                    if (model.Nazvanie)
                    {//если значение ячейки не равно нуль
                        if (excelworksheet.Cells[str, inazv].Value != null)
                        {
                            m.Nazvanie = excelworksheet.Cells[str, inazv].Value2.ToString();
                        }                                   
                    }
                    else
                    {
                        if(vh!=null)
                        {
                            m.Nazvanie = vh.Nazvanie;
                        }
                    }
                   if (model.Proizvoditel)
                    {
                        if (excelworksheet.Cells[str, iproiz].Value != null)
                        {
                            m.Proizvoditel = excelworksheet.Cells[str, iproiz].Value2.ToString();
                        }
                    }
                    else
                    {
                        if (vh != null)
                        {
                            m.Proizvoditel = vh.Proizvoditel;
                        }
                    }
                    if (model.StranaProizv)
                    {
                        if(excelworksheet.Cells[str, istrproiz].Value!=null)
                        {
                            m.StranaProizv = excelworksheet.Cells[str, istrproiz].Value2.ToString();
                        }                   
                    }
                    else
                    {
                        if (vh != null)
                        {
                            m.StranaProizv = vh.Strana;
                        }
                    }
                    if (model.Cvet)
                    {
                        if (excelworksheet.Cells[str, icvet].Value != null)
                        {
                            m.Cvet = excelworksheet.Cells[str, icvet].Value2.ToString();
                        }
                    }
                    else
                    {
                        if (vh != null)
                        {
                            m.Cvet = vh.Cvet;
                        }
                    }
                    if (model.Napolnitel)
                    {
                        if (excelworksheet.Cells[str,inapolnt].Value != null)
                        {
                            m.Napolnitel = excelworksheet.Cells[str,inapolnt ].Value2.ToString();
                        }
                    }
                    else
                    {
                        if (vh != null)
                        {
                            m.Napolnitel = vh.Napolnitel;
                        }
                    }
                    if (model.Yplotnitel)
                    {
                        if (excelworksheet.Cells[str,iyplotn].Value != null)
                        {
                            m.Yplotnitel = excelworksheet.Cells[str,iyplotn].Value2.ToString();
                        }
                    }
                    else
                    {
                        if (vh != null)
                        {
                            m.Yplotnitel = vh.Yplotnitel;
                        }
                    }
                    if (model.TolschinaMetala)
                    {
                        if (excelworksheet.Cells[str,itolmet].Value != null)
                        {
                            m.TolschinaMetala = double.Parse(excelworksheet.Cells[str,itolmet].Value2.ToString());
                        }
                    }
                    else
                    {
                        if (vh != null)
                        {
                            m.TolschinaMetala = vh.TolschinaMetalla;
                        }
                    }
                    if (model.TolschinaDvPolotna)
                    {
                        if (excelworksheet.Cells[str,itoldvpol].Value != null)
                        {
                            m.TolschinaDvPolotna = double.Parse(excelworksheet.Cells[str,itoldvpol].Value2.ToString());
                        }
                    }
                    else
                    {
                        if (vh != null)
                        {
                            m.TolschinaDvPolotna = vh.TolschinaDvPolotna;
                        }
                    }
                    if (model.Furnitura)
                    {
                        if (excelworksheet.Cells[str,ifur].Value != null)
                        {
                            m.Furnitura = excelworksheet.Cells[str,ifur].Value2.ToString();
                        }
                    }
                    else
                    {
                        if (vh != null)
                        {
                            m.Furnitura = vh.Furnitura;
                        }
                    }
                    if (model.Petli)
                    {
                        if (excelworksheet.Cells[str,ipetl].Value != null)
                        {
                            m.Petli = excelworksheet.Cells[str,ipetl].Value2.ToString();
                        }
                    }
                    else
                    {
                        if (vh != null)
                        {
                            m.Petli = vh.Petli;
                        }
                    }
                    if (model.OtdSnarugi)
                    {
                        if (excelworksheet.Cells[str,iotdsnar].Value != null)
                        {
                            m.OtdSnarugi = excelworksheet.Cells[str,iotdsnar].Value2.ToString();
                        }
                    }
                    else
                    {
                        if (vh != null)
                        {
                            m.OtdSnarugi = vh.OtdelkaSnarugi;
                        }
                    }
                    if (model.OtdVnutri)
                    {
                        if (excelworksheet.Cells[str,iotdvn].Value != null)
                        {
                            m.OtdVnutri = excelworksheet.Cells[str,iotdvn].Value2.ToString();
                        }
                    }
                    else
                    {
                        if (vh != null)
                        {
                            m.OtdVnutri = vh.OtdelkaVnutri;
                        }
                    }
                    if (model.Cena)
                    {
                        if (excelworksheet.Cells[str,icena].Value != null)
                        {
                            string cena = excelworksheet.Cells[str, icena].Value2.ToString();
                            if(cena == "Цена не установлена")
                            {
                                m.Cena = 0;
                            }
                            else
                            {
                                //парсим строку по символу "р"
                                string[] val = cena.Split('р');
                                m.Cena = decimal.Parse(val[0]);
                            }
                           
                        }
                    }
                    else
                    {
                        if (vh != null)
                        {
                            m.Cena = vh.Cena;
                        }
                    }
                    if (model.Skidka)
                    {
                        if (excelworksheet.Cells[str,iskid].Value != null)
                        {
                            m.Skidka = int.Parse(excelworksheet.Cells[str,iskid].Value2.ToString());

                                if (m.Skidka != 0 && (m.Cena != 0 && m.Cena != null))
                                {
                                    CenaSoSkidkoy = m.Cena - m.Cena * m.Skidka / 100;
                                }
                                else
                                {
                                    m.Skidka = null;
                                }
                        }
                    }
                    else
                    {
                        if (vh != null)
                        {
                            m.Skidka = vh.Skidka;
                        }
                        if (m.Skidka != 0 && (m.Cena != 0 && m.Cena != null))
                        {
                            CenaSoSkidkoy = m.Cena - m.Cena * m.Skidka / 100;
                        }
                    }
                    if (model.Publicaciya)
                    {
                        if (excelworksheet.Cells[str,ipubl].Value != null)
                        {
                            string pub = excelworksheet.Cells[str,ipubl].Value2.ToString();
                            if (pub == "ЛОЖЬ" || pub == "False" || pub == "false" || pub == "0" || pub == "непубликовать")
                            {
                                m.Publicaciya = false;
                            }
                            else
                            {
                                m.Publicaciya = true;
                            }

                        }
                        else
                        {
                            m.Publicaciya = true;
                        }
                    }
                    else
                    {
                        if (vh != null)
                        {
                            m.Publicaciya = vh.Publicaciya;
                        }
                    }
                    if (model.Opisanie)
                    {
                        if (excelworksheet.Cells[str,iopis].Value != null)
                        {
                            m.Opisanie = excelworksheet.Cells[str,iopis].Value2.ToString();
                        }
                    }
                    else
                    {
                        if (vh != null)
                        {
                            m.Opisanie = vh.Opisanie;
                        }
                    }
                    if (model.DopChar)
                    {
                        if (excelworksheet.Cells[str,idopch].Value != null)
                        {
                            m.DopChar = excelworksheet.Cells[str,idopch].Value2.ToString();
                        }
                    }
                    else
                    {
                        if (vh != null)
                        {
                            m.DopChar = vh.DopCharacteristics;
                        }
                    }
                    //создаем(изменяем) элемент в БД
                    dataManager.VhodnyeDvRepository.CreateVhodnyeDv(ID, m.Nazvanie, m.Proizvoditel, m.StranaProizv, m.Cvet, m.Napolnitel,
                     m.Yplotnitel, m.TolschinaMetala, m.Furnitura, m.Petli, m.OtdSnarugi, m.OtdVnutri, m.TolschinaDvPolotna,
                     m.Cena, m.Skidka, CenaSoSkidkoy, m.Opisanie, m.Publicaciya, m.DopChar);
                    //увеличили количество строк
                    str++;
                }
                KillExcel();
                return true;
            }
            catch(Exception er)
            {
                ClassLog.Write("ExpImpController/Import(ImportToDataBase)- " + er);
                return false;
            }

        }

        //метод удаления файлов из каталога
        private void DellAllFiles()
        {
            try
            {
                string domainpath = Server.MapPath("~//App_Data//ForDB//");
                var dir = new DirectoryInfo(domainpath);
                FileInfo[] fileNames = dir.GetFiles("*.*");
                foreach (var item in fileNames)
                {
                    item.Delete();
                }
            }
            catch(Exception er)
            {
                ClassLog.Write("ExpImpController/Import(DellAllFiles)- " + er);
            }
        }
        //вьюшка для вывода если в файле нехватает данных
        [HttpGet]
        public ActionResult EmptyFile()
        {
            return View();
        }
    }
}