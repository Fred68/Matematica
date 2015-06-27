using System;
using System.Collections.Generic;
using System.Text;
using Fred68.Tools.Matematica;
using Fred68.Tools.Utilita;

namespace Fred68.Tools.Matematica
	{
	/// <summary>
	/// Classe per trasformazioni geometriche 2D
	/// </summary>
	public class Transform2D : Matrix, ITextFile
		{
		#pragma warning disable 1591
		protected static int d2Dhom = 3;
		public static int Dim2Dhom {get {return d2Dhom;}}
		#pragma warning restore 1591
		#region COSTRUTTORI
		/// <summary>
		/// Costruttore principale
		/// Matrice identita`
		/// </summary>
		public Transform2D() : base(d2Dhom, d2Dhom)									
			{
			for(int i=0; i<d2Dhom; i++)
				this.Set(i,i,1.0);
			}
		/// <summary>
		/// Costruttore di copia da classe base
		/// </summary>
		/// <param name="m"></param>
		public Transform2D(Matrix m) : base(m,true)
			{	}
		#endregion
		#region MATRICI DI TRASFORMAZIONE
		/// <summary>
		/// Restituisce trasformazione di scala
		/// </summary>
		/// <param name="sx"></param>
		/// <param name="sy"></param>
		/// <returns></returns>
		public static Transform2D Scala(double sx, double sy)
			{
			Transform2D tr = new Transform2D();
			tr.Set(0,0,sx);
			tr.Set(1,1,sy);
			return tr;			
			}
		/// <summary>
		/// Restituisce trasformazione di scala uniforme
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static Transform2D Scala(double s)
			{
			return Transform2D.Scala(s,s);
			}
		/// <summary>
		/// Rotazione attorno all'origine
		/// </summary>
		/// <param name="alf"></param>
		/// <param name="radianti"></param>
		/// <returns></returns>
		public static Transform2D Rotazione(double alf, bool radianti = true)
			{
			Transform2D tr = new Transform2D();
			if(radianti == false)
				alf = alf * Math.PI/180.0;
			double cosalf = Math.Cos(alf);
			double sinalf = Math.Sin(alf);
			tr.Set(0,0,cosalf);
			tr.Set(1,1,cosalf);
			tr.Set(0,1,-sinalf);
			tr.Set(1,0,sinalf);
			return tr;
			}
		/// <summary>
		/// Traslazione
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public static Transform2D Traslazione(double x, double y)
			{
			Transform2D tr = new Transform2D();
			tr.Set(0,2,x);
			tr.Set(1,2,y);
			return tr;
			}
		/// <summary>
		/// Traslazione
		/// </summary>
		/// <param name="tr"></param>
		/// <returns></returns>
		public static Transform2D Traslazione(Point2D tr)
			{
			return Transform2D.Traslazione(tr.x,tr.y);
			}
		/// <summary>
		/// Combinazione di trasformazioni
		///  a+b: prima la a, poi la b
		/// </summary>
		/// <param name="sx"></param>
		/// <param name="dx"></param>
		/// <returns></returns>
		public static Transform2D operator+(Transform2D sx, Transform2D dx)
		    {
		    return new Transform2D( ((Matrix)dx) * ((Matrix)sx) );
		    }
		/// <summary>
		/// Rotazione attorno ad un punto
		/// Combinazione di traslazione inversa nell'origine, rotazione e traslazione
		/// </summary>
		/// <param name="p"></param>
		/// <param name="alf"></param>
		/// <param name="radianti"></param>
		/// <returns></returns>
		public static Transform2D Rotazione(Point2D p, double alf, bool radianti = true)
			{
			Transform2D tr1 = Transform2D.Traslazione(-p);
			Transform2D tr2 = Transform2D.Rotazione(alf, radianti);
			Transform2D tr3 = Transform2D.Traslazione(p);
			return tr1+tr2+tr3;
			#warning Rotazione attorno ad un punto: da verificare
			}
		#endregion
		#region OPERAZIONI SU PUNTI
		/// <summary>
		/// Trasforma da Point2D a coordinate omogenee
		/// </summary>
		/// <param name="pt"></param>
		/// <returns></returns>
		public static Matrix Convert(Point2D pt)
			{
			Matrix hom = new Matrix(d2Dhom,1);
			for(int i = 0; i < Point2D.Dim2D; i++)
				hom.Set(i,0,((Matrix)pt).Get(i,0));
			hom.Set(d2Dhom-1,0,1.0);
			return hom;
			}
		/// <summary>
		/// Trasforma da coordinate omogenee a Point2D
		/// </summary>
		/// <param name="pth"></param>
		/// <returns></returns>
		public static Point2D Convert(Matrix pth)
			{
			Point2D pt = new Point2D();
			for(int i=0; i<Point2D.Dim2D; i++)
				pt.Set(i,0,pth.Get(i,0));
			return pt;
			}
		/// <summary>
		/// Applica trasformazione al punto p
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public Point2D Transform(Point2D p)
			{
			Point2D ptr;
			Matrix phom, ptrhom;
			phom = Transform2D.Convert(p);		// Punto in coordinate omogenee
			ptrhom = this * phom;				// Applica trasformazione (prodotto)
			ptr = Transform2D.Convert(ptrhom);
			return ptr;
			}
		#endregion
		
		}
	}