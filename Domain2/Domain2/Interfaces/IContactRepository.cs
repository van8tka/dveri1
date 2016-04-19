﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALdv1;

namespace Domain2.Interfaces
{
  public interface IContactRepository
    {
        IEnumerable<Contact> GetContacts();
        IEnumerable<GrafikWork> GetGrafikWork();
        IEnumerable<Adresa> GetAdres();
        void DellContact(int id);
        void DellGrafikWork(int id);
        void DellAdres(int id);
        void CreateContact(int id, string type, string num);
        void CreateGrafikWork(int id, string day, string time);
        void CreateAdres(int id, string adres);
    }
}
