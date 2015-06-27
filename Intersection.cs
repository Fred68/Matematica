using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using Fred68.Tools.Matematica;
using Fred68.Tools.Grafica;

namespace Fred68.Tools.Matematica
	{
	/// <summary> Intersezione. Classe semplice di appoggio </summary>
	public class Intersection 
		{
		/// <summary>
		/// Punto di intersezione
		/// </summary>
		public Point2D p;
		/// <summary>
		/// Parametro della prima curva
		/// </summary>
		public double t1;
		/// <summary>
		/// Parametro della seconda curva
		/// </summary>
		public double t2;
		/// <summary>
		/// true se tangente (intersezione doppia) con il primo tratto
		/// </summary>
		public bool tg1;
		/// <summary>
		/// true se tangente (intersezione doppia) con il secondo tratto
		/// </summary>
		public bool tg2;
		/// <summary>
		/// Riferimento al primo tratto
		/// </summary>
		public Tratto tr1;
		/// <summary>
		/// Riferimento al secondo tratto
		/// </summary>
		public Tratto tr2;
		/// <summary>
		/// Costruttore
		/// </summary>
		/// <param name="pt"></param>
		/// <param name="par1"></param>
		/// <param name="par2"></param>
		/// <param name="tratto1"></param>
		/// <param name="tratto2"></param>
		/// <param name="tang1"></param>
		/// <param name="tang2"></param>
		public Intersection(Point2D pt, double par1, double par2, Tratto tratto1, Tratto tratto2, bool tang1=false, bool tang2 = false) 
			{
			p = pt;
			t1 = par1;
			t2 = par2;
			tg1 = tang1;
			tg2 = tang2;
			tr1 = tratto1;
			tr2 = tratto2; 
			}
		}
	}

