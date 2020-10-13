using System;
using System.Collections.Generic;
using System.Text;

namespace sands_arena
{
class Particle
{

    public Boolean turn;
    public int type;

    public Particle(int type, Boolean turn)
    {
        this.turn = turn;
        this.type = type;
    }

}
}
