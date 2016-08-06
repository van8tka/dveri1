using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALdv1;
using Domain2.Interfaces;

namespace Domain2.Implementations
{
    public class EfMegkomnatnyeDvRepository : IMegkomnatnyeDvRepository
    {
        private dbdveri1Entities1 context;
        public EfMegkomnatnyeDvRepository(dbdveri1Entities1 context)
        {
            this.context = context;
        }
        public void CreateFotoMkDv(int id, int iddv, string Type, byte[] Image)
        {
            if (id == 1)
            {
                FotoMegkomnDverey fvd = new FotoMegkomnDverey()
                {
                    IdFoto = id,
                    Id = iddv,
                    MimeType = Type,
                    Imaging = Image
                };
                context.FotoMegkomnDvereys.Add(fvd);
                context.SaveChanges();
            }
        }

        public void CreateMkDv(int id, string naz, string pr, string strpr, string cvet, string material, string pokryt, string karkas, string typdv, string vnytrnapoln, decimal? cena, int? skidka, decimal? csskid, string opis, bool publ, string dopchar)
        {
            if (id == 0)
            {
                MegkomnatnyeDveri vd = new MegkomnatnyeDveri()
                {
                    Id = id,
                    Nazvanie = naz,
                    Proizvoditel = pr,
                    Strana = strpr,
                    Cvet = cvet,
                    Material = material,
                    Pokrytie = pokryt,
                    Karkas = karkas,
                    TypDveri = typdv,
                    VnytrenneeNapolnenie= vnytrnapoln,
                    Cena = cena,
                    Skidka = skidka,
                    CenaSoSkidkoy = csskid,
                    Opisanie = opis,
                    Publicaciya = publ,
                    DopCharacteristics = dopchar
                };
                SaveMkDv(vd);
            }
            else
            {
                MegkomnatnyeDveri vd = context.MegkomnatnyeDveris.Where(x => x.Id == id).FirstOrDefault();
                vd.Id = id;
                vd.Nazvanie = naz;
                vd.Proizvoditel = pr;
                vd.Strana = strpr;
                vd.Cvet = cvet;
                vd.Material = material;
                vd.Pokrytie = pokryt;
                vd.Karkas = karkas;
                vd.TypDveri = typdv;
                vd.VnytrenneeNapolnenie = vnytrnapoln;
                vd.Cena = cena;
                vd.Skidka = skidka;
                vd.CenaSoSkidkoy = csskid;
                vd.Opisanie = opis;
                vd.Publicaciya = publ;
                vd.DopCharacteristics = dopchar;
                context.Entry(vd).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void CreateSeoMkDveri(int id, string title, string keywords, string description)
        {
            if (context.SeoMkDvereis.Find(id) != null)
            {
                SeoMkDverei se = context.SeoMkDvereis.Where(i => i.Id == id).FirstOrDefault();
                se.TitleDveri = title;
                se.KeywordsDveri = keywords;
                se.DescriptionDveri = description;
                context.Entry(se).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
            else
            {
                SeoMkDverei se = new SeoMkDverei
                {
                    Id = id,
                    TitleDveri = title,
                    KeywordsDveri = keywords,
                    DescriptionDveri = description
                };
                context.SeoMkDvereis.Add(se);
                context.SaveChanges();
            }
        }

        public void DeleteFotoMkDv(int idfoto)
        {
            context.FotoMegkomnDvereys.Remove(context.FotoMegkomnDvereys.Where(i => i.IdFoto == idfoto).FirstOrDefault());
            context.SaveChanges();
        }

        public void DeleteMkDv(int id)
        {
            context.MegkomnatnyeDveris.Remove(GetMkDvById(id));
            context.SaveChanges();
        }

        public IEnumerable<FotoMegkomnDverey> GetFotoMkDv()
        {
            return context.FotoMegkomnDvereys;
        }

        public IEnumerable<FotoMegkomnDverey> GetFotoMkDvByID(int id)
        {
            return context.FotoMegkomnDvereys.Where(i => i.Id == id);
        }

        public IEnumerable<MegkomnatnyeDveri> GetMkDv()
        {
            return context.MegkomnatnyeDveris;
        }

        public MegkomnatnyeDveri GetMkDvById(int id)
        {
            return context.MegkomnatnyeDveris.Where(i => i.Id == id).FirstOrDefault();
        }

        public IEnumerable<SeoMkDverei> GetSeoMkDv()
        {
            return context.SeoMkDvereis;
        }

        public void SaveMkDv(MegkomnatnyeDveri temp)
        {
            context.MegkomnatnyeDveris.Add(temp);
            context.SaveChanges();
        }
    }
}
