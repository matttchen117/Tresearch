using System;
using System.Collections.Generic;
using System.Text;

namespace TrialByFire.Tresearch.DAL
{
    public interface IRepository<T>
    {
        bool Create(T model);

        bool Read();

        bool Update(T model);

        bool Delete(T model);


    }
}
