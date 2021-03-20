using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstates.Services
{
    public interface ITagService
    {
        void Add(string name, int? importance);
        void BulkTagProperties();
    }
}
