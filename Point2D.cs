using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using Fred68.Tools.Matematica;
using Fred68.Tools.Utilita;
using System.IO;

namespace Fred68.Tools.Matematica
	{
	/// <summary> Point2D class </summary>
	public class Point2D : Matrix //, ITextFile
		{
		#pragma warning disable 1591
		protected static int d2D = 2;
		public static int Dim2D {get {return d2D;}}
		#pragma warning restore 1591
		#region COSTRUTTORI
		/// <summary>
		/// Punto
		/// </summary>
		public Point2D() : base(d2D, 1)									
			{
			}
		/// <summary>
		/// Punto
		/// </summary>
		/// <param name="matrixinfo">Tipo di matrice</param>
		//public Point2D(MatrixInfo matrixinfo) : base (d2D, 1)			
		//	{
		//	this.matrixInfo = matrixinfo;
		//	}
		/// <summary>
		/// Punto (copia di altro punto)
		/// </summary>
		/// <param name="m">Matrice originaria</param>
		/// <param name="copy">true: copia i dati</param>
		public Point2D(Point2D m, bool copy) : base(m, copy)			
			{
			}
		/// <summary>
		/// Punto
		/// </summary>
		/// <param name="x">Coordinata x</param>
		/// <param name="y">Coordinata y</param>
		public Point2D(double x, double y) :  base (d2D, 1)				
			{
			this[0, 0] = x;
			this[1, 0] = y;
			}
		/// <summary>
		/// Punto
		/// </summary>
		/// <param name="length">Modulo</param>
		/// <param name="angolo">Angolo</param>
		/// <param name="radianti">true: radianti, false: gradi</param>
		public Point2D(double length, double angolo, bool radianti) :  base (d2D, 1) 
			{
			double ang;
			if(radianti)
				ang = angolo;
			else
				ang = angolo * Math.PI / 180.0;
			this[0, 0] = length*Math.Cos(ang);
			this[1, 0] = length*Math.Sin(ang);
			}
		#endregion
		#region PROPRIETA
		/// <summary>
		/// Coordinata x
		/// </summary>
		public double x													// Coordinata x 
			{
			get {return this[0, 0]; }
			set {this[0, 0] = value; }

			}
		/// <summary>
		/// Coordinata y
		/// </summary>
		public double y													// Coordinata y 
			{
			get { return this[1, 0]; }
			set { this[1, 0] = value; }
			}
		#endregion
		#region OPERATORI e FUNZIONI
		/// <summary>
		/// Normalizza il vettore, se possibile
		/// </summary>
		/// <returns>true se avvenuta normalizzazione</returns>
		public bool Normalize() 
			{
			double mod = this.Mod();
			if(mod > double.Epsilon)
				{
				this.x = this.x / mod;
				this.y = this.y / mod;
				return true;
				}
			else
				return false;
			}
		/// <summary>
		/// Lunghezza (modulo)
		/// </summary>
		/// <returns>Il modulo</returns>
		public double Mod() 
			{
			return Math.Sqrt((this.x * this.x) + (this.y * this.y));
			}
		/// <summary>
		/// Angolo (fase)
		/// </summary>
		/// <param name="radianti">true: radianti, false: gradi</param>
		/// <returns>L'angolo calcolato</returns>
		public double Alfa(bool radianti = true) 
			{
			double a = Math.Atan2(y, x);
			if(radianti)
				return a;
			else
				return 180.0*a/Math.PI;
			}
		/// <summary>
		/// Versore (modulo unitario) parallelo
		/// </summary>
		/// <returns>Il versore o [0,0] se errore</returns>
		public Point2D Versor() 
			{
			Point2D n = new Point2D(this.x, this.y);
			if(n.Normalize())
				return n;
			else
				return new Point2D();				
			}
		/// <summary>
		/// Versore normale (ruotato +90 gradi)
		/// </summary>
		/// <returns>Il vettore perpendicolare, normalizzato a modulo unitario</returns>
		public Point2D Normal() 
			{
			Point2D n = new Point2D(- this.y, this.x);
			n.Normalize();
			return n;
			}
		/// <summary>
		/// Somma
		/// </summary>
		/// <param name="sx"></param>
		/// <param name="dx"></param>
		/// <returns></returns>
		public static Point2D operator +(Point2D sx, Point2D dx) 
		    {
		    return new Point2D(dx.x + sx.x, dx.y + sx.y);
		    }
		/// <summary>
		/// Differenza
		/// </summary>
		/// <param name="sx"></param>
		/// <param name="dx"></param>
		/// <returns></returns>
		public static Point2D operator -(Point2D sx, Point2D dx) 
		    {
			return new Point2D(sx.x - dx.x, sx.y - dx.y);
		    }
		/// <summary>
		/// Prodotto scalare
		/// </summary>
		/// <param name="sx"></param>
		/// <param name="dx"></param>
		/// <returns></returns>
		public static double operator ^(Point2D sx, Point2D dx) 
		    {
			return dx.x * sx.x + dx.y * sx.y;
		    }
		/// <summary>
		/// Prodotto Punto * numero
		/// </summary>
		/// <param name="sx"></param>
		/// <param name="dx"></param>
		/// <returns></returns>
		public static Point2D operator *(Point2D sx, double dx) 
		    {
			return new Point2D(sx.x * dx, sx.y * dx);
			}
		/// <summary>
		/// Prodotto numero * Punto
		/// </summary>
		/// <param name="sx"></param>
		/// <param name="dx"></param>
		/// <returns></returns>
		public static Point2D operator *(double sx, Point2D dx) 
			{
			return new Point2D(sx * dx.x, sx * dx.y);
			}
		/// <summary>
		/// Quoziente Punto / numero (Matrixinfo errore se divisione per zero)
		/// </summary>
		/// <param name="sx"></param>
		/// <param name="dx"></param>
		/// <returns></returns>
		public static Point2D operator /(Point2D sx, double dx) 
		    {
			if (System.Math.Abs(dx) <= Matrix.Epsilon)
				{
				throw new InvalidOperationException(Error.DivisionByZero.ToString());		// Se divisione per zero: eccezione
				}
			return new Point2D(sx.x / dx, sx.y / dx);
			}
		/// <summary>
		/// Cambio di segno
		/// </summary>
		/// <param name="dx"></param>
		/// <returns></returns>
		public static Point2D operator -(Point2D dx) 
			{
			return new Point2D(-dx.x, -dx.y);
			}
		/// <summary>
		/// ToString()
		/// </summary>
		/// <returns></returns>
		public override string ToString() 
			{
			StringBuilder str = new StringBuilder("[");
			str.Append(this.x);
			str.Append(';');
			str.Append(this.y);
			str.Append(']');
			return str.ToString();
			}
		#endregion
		#region IOSTREAM
		#warning Operazioni su stream DA STUDIARE E SCRIVERE. Occhio ai token e alle parole chiave
		/// <summary> </summary>
		/// <param name="sw"></param>
		/// <returns></returns>
		public bool Scrivi(StreamWriter sw)
			{
			return false;
			}
		/// <summary></summary>
		/// <param name="sr"></param>
		/// <returns></returns>
		public bool Leggi(StreamReader sr)
			{
			return false;
			}
		#endregion
		}
	}
