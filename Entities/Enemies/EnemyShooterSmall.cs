/*
Enemy that shoots shadow bullets towards the player.
*/
using FlashBOOM.Entities.Players;
using FlashBOOM.Entities.Projectiles;
using Microsoft.Xna.Framework;
using System;

namespace FlashBOOM.Entities.Enemies
{
    public class EnemyShooterSmall : EnemyShooter 
    {
        // initial variables


        public override int EnemyWidth => 8; // experiment and change this
        public override int EnemyHeight  => 8; // experiment and change this



        public override void Initialize()
        {
            base.Initialize();
        
        }

      
       
      

          

        
    }
    
}