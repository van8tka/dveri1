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
        private IUserRepository userRepository;
        private ISeoMainRepository seoMainRepository;
        private ICommentRepository commentRepository;
        private IYstanovkaRepository ystanovkaRepository;
        private IArticlesRepository articlesRepository;

        public DataManager(IArticlesRepository articlesRepository, IYstanovkaRepository ystanovkaRepository, ICommentRepository commentRepository, ISeoMainRepository seoMainRepository, IUserRepository userRepository, IOplataDostavkaRepository opldostRepository, IKlientRepository klientRepository, IVhodnyeDvRepository vhodnDvRepository, ISliderRepository sliderRepository, IContactRepository contactRepository)
        {
            this.articlesRepository = articlesRepository;
            this.klientRepository = klientRepository;
            this.vhodnDvRepository = vhodnDvRepository;
            this.sliderRepository = sliderRepository;
            this.contactRepository = contactRepository;
            this.opldostRepository = opldostRepository;
            this.userRepository = userRepository;
            this.seoMainRepository = seoMainRepository;
            this.commentRepository = commentRepository;
            this.ystanovkaRepository = ystanovkaRepository;
        }
        //св-ва через которые будет происх вызов
        public IArticlesRepository ArticlesRepository
        {
            get { return articlesRepository; }
        }
        public IYstanovkaRepository YstanovkaRepository
        {
            get { return ystanovkaRepository; }
        }
        public ICommentRepository CommentRepository
        {
            get { return commentRepository; }
        }
        public ISeoMainRepository SeoMainRepository
        {
            get { return seoMainRepository; }
        }
        public IUserRepository UserRepository
        {
            get { return userRepository; }
        }
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
