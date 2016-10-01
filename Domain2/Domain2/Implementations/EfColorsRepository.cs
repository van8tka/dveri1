using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALdv1;
using Domain2.Interfaces;

namespace Domain2.Implementations
{
    public class EfColorsRepository : IColorsRepository
    {
        private dbdveri1Entities1 context;
        public EfColorsRepository(dbdveri1Entities1 context)
        {
            this.context = context;
        }
        public void CreateColor(int idcolor, string namecolor, string type, byte[] image)
        {
            if (context.TableColors.Find(idcolor) != null)
            {
                TableColor tc = context.TableColors.Where(x => x.IdColor == idcolor).FirstOrDefault();
                tc.NameColor = namecolor;
                tc.MimeType = type;
                tc.Image = image;
                context.Entry(tc).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
            else
            {
                TableColor tc = new TableColor()
                {
                    IdColor = idcolor,
                    NameColor = namecolor,
                    MimeType = type,
                    Image = image
                };
                context.TableColors.Add(tc);
                context.SaveChanges();
            }
        }

        public void DelColor(int idcolor)
        {
            context.TableColors.Remove((context.TableColors.Where(x => x.IdColor == idcolor).FirstOrDefault()));
            context.SaveChanges();
        }

        public TableColor GetColor(int idcolor)
        {
            return context.TableColors.Where(x => x.IdColor == idcolor).FirstOrDefault();
        }

        public IEnumerable<TableColor> GetColors()
        {
            return context.TableColors;
        }
    }
}
