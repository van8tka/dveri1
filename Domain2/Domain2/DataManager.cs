using Domain2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain2
{
    public class DataManager
    {
        //туту будут описаны связи
        private IVhodnyeDvRepository vhodnDvRepository;
        private ISliderRepository sliderRepository;
        private IContactRepository contactRepository;
        private IKlientRepository klientRepository;
        private IOplataDostavkaRepository opldostRepository;

        public DataManager(IOplataDostavkaRepository opldostRepository, IKlientRepository klientRepository, IVhodnyeDvRepository vhodnDvRepository, ISliderRepository sliderRepository, IContactRepository contactRepository)
        {
            this.klientRepository = klientRepository;
            this.vhodnDvRepository = vhodnDvRepository;
            this.sliderRepository = sliderRepository;
            this.contactRepository = contactRepository;
            this.opldostRepository = opldostRepository;
        }
        //св-ва через которые будет происх вызов
        public IOplataDostavkaRepository OplDostRepository
        {
            get { return opldostRepository; }
        }
        public IKlientRepository KlientRepository
        {
            get { return klientRepository; }
        }
        public IVhodnyeDvRepository VhodnyeDvRepository
        {
            get { return vhodnDvRepository; }
        }
        public ISliderRepository SliderRepository
        {
            get { return sliderRepository; }
        }
        public IContactRepository ContactRepository
        {
            get { return contactRepository; }
        }

    }
}
