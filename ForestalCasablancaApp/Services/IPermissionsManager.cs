using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForestalCasablancaApp.Services
{
    public interface IPermissionsManager
    {
        Task GetPermissionsAsync();
    }
}
