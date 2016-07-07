using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALdv1;
using Domain2.Interfaces;

namespace Domain2.Implementations
{
   public class EfYstanovkaRepository:IYstanovkaRepository
    {
        dbdveri1Entities1 context;
        public EfYstanovkaRepository(dbdveri1Entities1 context)
        {
            this.context = context;
        }

        public void CreateYstanovka(int id, string yst)
        {
            if(id == 0)
            {
                TableYstanovka ty = new TableYstanovka()
                {
                    ID = id,
                    Ystanovka = yst
                };
                TableYstanovka existty = context.TableYstanovkas.FirstOrDefault();
                if(existty!=null)
                {
                    DelYstanovka(existty.ID);
                }
                context.TableYstanovkas.Add(ty);
                context.SaveChanges();
            }
            else
            {
                TableYstanovka ty = context.TableYstanovkas.Where(z => z.ID == id).FirstOrDefault();
                ty.Ystanovka = yst;
                
            }
        }

        public void DelYstanovka(int id)
        {
            context.TableYstanovkas.Remove(context.TableYstanovkas.Where(z => z.ID == id).FirstOrDefault());
            context.SaveChanges();
        }

        public IEnumerable<TableYstanovka> GetYstanovka()
        {
            return context.TableYstanovkas;
        }
    }
}
