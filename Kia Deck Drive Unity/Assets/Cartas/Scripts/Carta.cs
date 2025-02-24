using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KDDC
{
    [CreateAssetMenu(fileName = "Nueva Carta", menuName ="Carta")]
    public class Carta : ScriptableObject
    {
        public string CartaNombre;
        public List<TipoCarta> tipocarta;
        public int salud;
        public int damageMin;
        public int damageMax;
        public int escudo;
        public int Costo;
        public string descripcion;
        public Sprite imagen;
        public List<DamageType> damageType;
        public enum TipoCarta
        {
            Vida,
            Escudo,
            Damage,
            Especial

        }

        public enum DamageType
        {
            Debuff,
            Retraso,
            Seguir
        }
    }
}