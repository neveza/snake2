using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1
{

    class UserInput
    {
       static public float getYAxis()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                return -1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                return 1;
            }
            return 0;
        }
       static public float getXAxis()
        {
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    return 1;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    return -1;
                }
                return 0;
            
        }
    }
}
