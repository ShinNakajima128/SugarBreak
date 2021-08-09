using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.System.Scripts.Damage
{
    public interface IDamagable
    {
        void Damage(int attackPower);
    }
}
