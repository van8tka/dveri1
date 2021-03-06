﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALdv1;
using Domain2.Interfaces;

namespace Domain2.Implementations
{
   public class EfVhodnyeDvRepository:IVhodnyeDvRepository
    {
        private dbdveri1Entities1 context;
        public EfVhodnyeDvRepository(dbdveri1Entities1 context)
        {
            this.context = context;
        }

        public void CreateFotoVhDv(int id,int iddv, string Type, byte[] Image)
        {
            if(id==1)
            {
                FotoVhodnyhDverey fvd = new FotoVhodnyhDverey()
                {
                    Idfoto = id,
                    Id = iddv,
                    MimeType = Type,
                    Imaging = Image
                };
                context.FotoVhodnyhDvereys.Add(fvd);
                context.SaveChanges();
            }
        }

        public void CreateVhodnyeDv(int id, string naz, string pr, string strpr, int? cvet, string napoln, string yplotnit, double? tmet, string furn, string petli, string osn, string ovn, double? tdpol, decimal? cena, int? skidka, decimal? csskid, string opis, bool publ, string dopchar)
        {
            if (id == 0)
            {
                VhodnyeDveri vd = new VhodnyeDveri()
                {
                    Id = id,
                    Nazvanie = naz,
                    Proizvoditel = pr,
                    Strana = strpr,
                    IdColor = cvet,
                    Napolnitel = napoln,
                    Yplotnitel = yplotnit,
                    TolschinaMetalla = tmet,
                    Furnitura = furn,
                    Petli = petli,
                    OtdelkaSnarugi = osn,
                    OtdelkaVnutri = ovn,
                    TolschinaDvPolotna = tdpol,
                    Cena = cena,
                    Skidka = skidka,
                    CenaSoSkidcoy = csskid,
                    Opisanie = opis,
                    Publicaciya = publ,
                    DopCharacteristics = dopchar
                };
                SaveVhodnyeDv(vd);
            }
            else
            {
                VhodnyeDveri vd = context.VhodnyeDveris.Where(x => x.Id == id).FirstOrDefault();             
                vd.Id = id;
                vd.Nazvanie = naz;
                vd.Proizvoditel = pr;
                vd.Strana = strpr;
                vd.IdColor = cvet;
                vd.Napolnitel = napoln;
                vd.Yplotnitel = yplotnit;
                vd.TolschinaMetalla = tmet;
                vd.Furnitura = furn;
                vd.Petli = petli;
                vd.OtdelkaSnarugi = osn;
                vd.OtdelkaVnutri = ovn;
                vd.TolschinaDvPolotna = tdpol;
                vd.Cena = cena;
                vd.Skidka = skidka;
                vd.CenaSoSkidcoy = csskid;
                vd.Opisanie = opis;
                vd.Publicaciya = publ;
                vd.DopCharacteristics = dopchar;
                context.Entry(vd).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteFotoVhDv(int idfoto)
        {

            context.FotoVhodnyhDvereys.Remove(context.FotoVhodnyhDvereys.Where(x=>x.Idfoto == idfoto).FirstOrDefault());
            context.SaveChanges();
        }

        public void DeleteVhodnyeDv(int id)
        {

            context.VhodnyeDveris.Remove(GetVhodnyeDvById(id));
            context.SaveChanges();
        }

        public IEnumerable<FotoVhodnyhDverey> GetFotoVhDvByID(int id)
        {
            return context.FotoVhodnyhDvereys.Where(x => x.Id == id);
        }
        public IEnumerable<FotoVhodnyhDverey> GetFotoVhDv()
        {
            return context.FotoVhodnyhDvereys;
        }

        public IEnumerable<VhodnyeDveri> GetVhodnyeDv()
        {
            return context.VhodnyeDveris;
        }

        public VhodnyeDveri GetVhodnyeDvById(int id)
        {
            return context.VhodnyeDveris.Where(x => x.Id == id).FirstOrDefault();
        }

        public void SaveVhodnyeDv(VhodnyeDveri temp)
        {
            context.VhodnyeDveris.Add(temp);
            context.SaveChanges();
        }
        public void CreateSeoVhDveri(int id, string title, string keywords, string description)
        {
           
            if (context.SeoVhodnuhDvereis.Find(id) != null)
            {
                SeoVhodnuhDverei se = context.SeoVhodnuhDvereis.Where(i => i.Id == id).FirstOrDefault();
                se.TitleDveri = title;
                se.KeywordsDveri = keywords;
                se.DescriptionDveri = description;
                context.Entry(se).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges(); 
            }
            else
            {
                SeoVhodnuhDverei se = new SeoVhodnuhDverei
                {
                    Id = id,
                    TitleDveri = title,
                    KeywordsDveri = keywords,
                    DescriptionDveri = description
                };
                context.SeoVhodnuhDvereis.Add(se);
                context.SaveChanges();
            }
        }
       public IEnumerable<SeoVhodnuhDverei> GetSeoVhDv()
        {
            return context.SeoVhodnuhDvereis;
        }
    }
}
