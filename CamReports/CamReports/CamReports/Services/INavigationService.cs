using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamReports.Services
{
    public interface INavigationService : GalaSoft.MvvmLight.Views.INavigationService
    {
        void BackToMain();
    }
}
