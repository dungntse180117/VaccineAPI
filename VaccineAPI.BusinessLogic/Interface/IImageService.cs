using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineAPI.BusinessLogic.Interface
{
    public interface IImageService
    {
        Task<bool> SaveImagesAsync(string imageUrl); // more than one file will save.
    }
}
