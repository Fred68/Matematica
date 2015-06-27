using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fred68.Tools.Matematica;

namespace Fred68.Tools.Matematica
	{
	class Vector2D
		{
		protected static int d2D = 2;
		public static int Dim2D { get { return d2D; } }

		protected Point2D orig;													// Origine
		protected Point2D vers;													// Versore
		protected bool valid;
		#region PROPRIETA
		public Point2D origin	{
								get { return orig; }				// Proprieta`
								set {
									orig = value;
									valid = Validate();
									}
								}
		public Point2D versor
								{
								get { return vers; }
								set
									{
									vers = value;
									valid = Validate();
									}
								}
		public double xorig
							{
							get { return orig.x; }
							set
								{
								orig.x = value;
								valid = Validate();
								}
							}
		public double yorig
							{
							get { return orig.y; }
							set
								{
								orig.y = value;
								valid = Validate();
								}
							}
		public double xvers
							{
							get { return vers.x; }
							set
								{
								vers.x = value;
								valid = Validate();
								}
							}
		public double yvers
							{
							get { return vers.y; }
							set
								{
								vers.y = value;
								valid = Validate();
								}
							}
		#endregion
		#region COSTRUTTORI
		public Vector2D()															// Senza argomenti
			{
			orig = new Point2D();
			vers = new Point2D();
			this.valid = this.Validate();
			}
		public Vector2D(Point2D orig, Point2D vers, bool bVers = true)			// Origine + versore o secondo punto
			{
			this.orig = orig;												
			if(bVers)															// Imposta il versore
				{
				this.vers = vers;
				}
			else
				{
				this.vers = vers - orig;										// Imposta la differenza
				}
			this.valid = this.Validate();
			}
		public Vector2D(double xorig, double yorig, double xvers, double yvers, bool bVers = true)	// Origine + versore o secondo punto
			{
			this.orig = new Point2D(xorig,yorig);
			if(bVers)
				{
				this.vers = new Point2D(xvers,yvers);							// Imposta il versore
				}
			else
				{
				this.vers = new Point2D(xvers - xorig, yvers - yorig);			// Imposta la differenza
				}
			this.valid = this.Validate();
			}
		public Vector2D(double xorig, double yorig, double angolo, bool radianti = true)	// Origine + angolo in radianti o gradi
			{
			double ang;
			this.orig = new Point2D(xorig, yorig);
			if(radianti)
				ang = angolo;
			else
				ang = angolo*Math.PI / 180.0;
			this.vers = new Point2D(Math.Cos(ang), Math.Sin(ang));
			this.valid = this.Validate();
			}
		#endregion
		public bool Validate()													// Verifica e normalizza il versore
			{
			if(Math.Abs(Point2D.Mod(this.vers) - 1.0) > double.Epsilon)			// Se il modulo del versore non e` unitario...
				{
				return this.vers.Normalize();									// ...lo normalizza (restituisce false se nullo)
				}
			else
				{
				return true;													// Altrimenti restituisce true, e` gia` normalizzato
				}
			}
		}
	}
