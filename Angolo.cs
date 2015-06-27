using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using Fred68.Tools.Matematica;
using Fred68.Tools.Utilita;
using System.IO;

namespace Fred68.Tools.Matematica
	{
	/// <summary>
	/// Classe per gestire gli angoli tra -PI e +PI in radianti
	/// </summary>
	public class Angolo
		{
		// Esempio in:		https://d3dkrwjzubdpha.cloudfront.net/wp-content/uploads/Angle.cs
		// Usare anche alfaIni = Math.IEEERemainder(alfaIni, PI2);		// Normalizza tra -PI e +PI
		#pragma warning disable 1591
		protected static readonly double PI2 = 2.0 * Math.PI;			// 2 pigreco
		protected double _angolo;
		#pragma warning restore 1591
		/// <summary>
		/// Costruttore (da double)
		/// </summary>
		/// <param name="ang"></param>
		public Angolo(double ang)
			{
			_angolo = Math.IEEERemainder(ang, PI2);
			}
		/// <summary>
		/// Conversione implicita da Angolo a double
		/// </summary>
		/// <param name="ang">Angolo</param>
		/// <returns></returns>
		public static implicit operator double(Angolo ang)
			{
			return ang._angolo;
			}	
		/// <summary>
		/// Conversione implicita da double ad Angolo
		/// </summary>
		/// <param name="ang">double</param>
		/// <returns></returns>
		public static implicit operator Angolo(double ang)
			{
			return new Angolo(ang);
			}
		/// <summary>
		/// Sottrazione a-b
		/// </summary>
		/// <param name="sx">a</param>
		/// <param name="dx">b</param>
		/// <returns></returns>
        public static Angolo operator -(Angolo sx, Angolo dx)
			{
            return new Angolo(sx._angolo - dx._angolo);
			}
		/// <summary>
		/// Somma a+b
		/// </summary>
		/// <param name="sx">a</param>
		/// <param name="dx">b</param>
		/// <returns></returns>
        public static Angolo operator +(Angolo sx, Angolo dx)
			{
            return new Angolo(sx._angolo + dx._angolo);
			}
		/// <summary>Confronto</summary>
        public static bool operator <(Angolo sx, Angolo dx)
			{
            if (sx._angolo < dx._angolo)
                return true;
            return false;
			}
		/// <summary>Confronto</summary>
        public static bool operator <=(Angolo sx, Angolo dx)
			{
            if (sx._angolo <= dx._angolo)
                return true;
            return false;
			}
		/// <summary>Confronto</summary>
        public static bool operator >(Angolo sx, Angolo dx)
			{
            if (sx._angolo > dx._angolo)
                return true;
            return false;
			}
		/// <summary>Confronto</summary>
        public static bool operator >=(Angolo sx, Angolo dx)
			{
            if (sx._angolo >= dx._angolo)
                return true;
            return false;
			}
		}
	}