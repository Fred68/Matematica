using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Fred68.Tools.Matematica;
using Fred68.Tools.Grafica;

namespace Fred68.Tools.Matematica
	{
	/// <summary> Linea </summary>
	public class Line2D : Tratto
		{
		#pragma warning disable 1591
		protected static int d2D = 2;
		public static int Dim2D { get { return d2D; } }
		protected static readonly double epsilon = System.Double.Epsilon;		// Epsilon
		public static double Epsilon { get { return epsilon; } }
		protected static readonly double coincidencedistance = 1E-12;								// Distanza minima due punti distinti
		public static double CoincidenceDistance { get { return coincidencedistance; } }
		#region PROTECTED
		protected Point2D p1;									// Origine
		protected Point2D p2;									// Versore
		protected double length;								// Lunghezza				
		protected bool normalized;								// Flag se normalizzato
		protected bool valid;									// Se lunghezza non nulla
		#endregion
		#pragma warning restore 1591
		#region PROPRIETA
		#pragma warning disable 1591
		public Point2D P1 
							{
							get { return p1; }				// Proprieta`
							set {
								p1 = value;
								this.Validate();
								}
							}
		public Point2D P2 
							{
							get { return p2; }
							set
								{
								p2 = value;
								this.Validate();
								}
							}
		public double x1 
							{
							get { return p1.x; }
							set
								{
								p1.x = value;
								this.Validate();
								}
							}
		public double y1 
							{
							get { return p1.y; }
							set
								{
								p1.y = value;
								this.Validate();
								}
							}
		public double x2 
							{
							get { return p2.x; }
							set
								{
								p2.x = value;
								this.Validate();
								}
							}
		public double y2 
							{
							get { return p2.y; }
							set
								{
								p2.y = value;
								this.Validate();
								}
							}
		public double Length 
							{
							get { return this.length; }
							}
		public override bool IsValid 
							{
							get { return this.valid; }
							}
		public bool IsNormalized 
							{
							get {return this.normalized;}
							}
		/// <summary>
		/// Punto iniziale
		/// </summary>
		public override Point2D pStart 
		    {
		    get {return P1;}
		    }
		/// <summary>
		/// Punto finale
		/// </summary>
		public override Point2D pEnd 
		    {
		    get {return P2;}
		    }
		/// <summary>
		/// Array con punto iniziale e finale
		/// </summary>
		public override Point2D[] P12 
			{
			get
				{
				Point2D[] ap = new Point2D[2];
				ap[0] = p1;
				ap[1] = p2;
				return ap;
				}		
			}
		#pragma warning restore 1591
		#endregion
		#region COSTRUTTORI
		/// <summary>
		/// Linea
		/// </summary>
		public Line2D() : base(typeof(Line2D)) 
			{
			p1 = new Point2D();
			p2 = new Point2D();
			this.Validate();
			}
		/// <summary>
		/// Costruttore di copia
		/// </summary>
		/// <param name="l">Linea</param>
		public Line2D(Line2D l) : base(typeof(Line2D)) 
			{
			this.p1 = l.p1;
			this.p2 = l.p2;
			Validate();
			}
		/// <summary>
		/// Linea
		/// </summary>
		/// <param name="p1">Primo punto</param>
		/// <param name="p2">Secondo punto</param>
		/// <param name="bRelative">true: secondo punto sommato al primo</param>
		public Line2D(Point2D p1, Point2D p2, bool bRelative = false) : base(typeof(Line2D)) 
			{
			if(bRelative)
				{
				this.p1 = p1;												
				this.p2 = this.p1 + p2;
				}
			else
				{
				this.p1 = p1;
				this.p2 = p2;
				}
			this.Validate();
			}
		/// <summary>
		/// Linea
		/// </summary>
		/// <param name="x1">x primo punto</param>
		/// <param name="y1">y primo punto</param>
		/// <param name="x2">x secondo punto</param>
		/// <param name="y2">y secondo punto</param>
		/// <param name="bRelative">true: secondo punto sommato al primo</param>
		public Line2D(double x1, double y1, double x2, double y2, bool bRelative = false) : base(typeof(Line2D)) 
			{
			if(bRelative)
				{
				this.p1 = new Point2D(x1,y1);
				this.p2 = new Point2D(p1.x + x2, p1.y + y2);
				}
			else
				{
				this.p1 = new Point2D(x1, y1);
				this.p2 = new Point2D(x2, y2);
				}
			this.Validate();
			}
		/// <summary>
		/// Linea
		/// </summary>
		/// <param name="x1">x primo punto</param>
		/// <param name="y1">y primo punto</param>
		/// <param name="angolo">angolo</param>
		/// <param name="length">lunghezza</param>
		/// <param name="radianti">tre: angolo in radianti, false: in gradi</param>
		/// <param name="bRelative">true: secondo punto sommato al primo</param>
		public Line2D(double x1, double y1, double angolo, double length, bool radianti, bool bRelative = true) : base(typeof(Line2D)) 
			{
			double ang;
			this.p1 = new Point2D(x1, y1);
			if(radianti)
				ang = angolo;
			else
				ang = angolo * Math.PI / 180.0;
			if(bRelative)
				this.p2 = this.P1 + length *(new Point2D(Math.Cos(ang), Math.Sin(ang)));
			else
				this.p2 = length * (new Point2D(Math.Cos(ang), Math.Sin(ang)));
			this.Validate();
			}
		#endregion
		#region FUNZIONI
		/// <summary>
		/// Calcola lunghezza e verifica. Aggiorna anche flag se normalizzato
		/// </summary>
		public override void Validate() 
			{
			length = (this.p2 - this.p1).Mod();
			if( Math.Abs(length - 1.0) <= coincidencedistance)
				this.normalized = true;	
			if(length >= double.Epsilon)
				this.valid = true;
			else
				this.valid = false;
			}
		/// <summary>
		/// Vettore P1 -> P2
		/// </summary>
		/// <returns>Il vettore</returns>
		public Point2D Vector() 
			{
			return p2 - p1;
			}
		/// <summary>
		/// Versore (modulo unitario) P1 -> P2
		/// </summary>
		/// <returns>Il versore</returns>
		public Point2D Versor() 
			{
			return (this.Vector()).Versor();
			}
		/// <summary>
		/// Normalizza tenendo fisso P1
		/// </summary>
		public void Normalize() 
			{
			Point2D v = p2-p1;					// Normalizza vettore da P1 a P2 e ricalcola P2.
			v.Normalize();		
			p2 = p1 + v;
			this.Validate();
			}
		/// <summary>
		/// Punto medio
		/// </summary>
		/// <returns>Il punto</returns>
		public Point2D Midpoint() 
			{
			return 0.5 * (p1 + p2);
			}
		/// <summary>
		/// Punto sulla linea calcolato con il parametro t
		/// </summary>
		/// <param name="t">parametro: se [0,1], e` tra P1 e P2</param>
		/// <returns>Il punto</returns>
		public Point2D Point(double t) 
			{
			return this.p1 + (t * (this.p2 - this.p1));
			}
		/// <summary>
		/// Linea d'asse
		/// </summary>
		/// <returns>La linea</returns>
		public Line2D Axis() 
			{
			Point2D dir = p2 - p1;
			Point2D mid = this.Midpoint();
			return new Line2D(mid, dir.Normal(), true);
			}
		/// <summary>
		/// Appartenenza del punto p alla retta
		/// </summary>
		/// <param name="p">Punto</param>
		/// <param name="bInside">true richiede appartenenza al segmento</param>
		/// <returns>true se appartiene alla retta o al segmento</returns>
		public override bool Belongs(Point2D p, bool bInside = false)
		    {
		    if (this.valid)
		        {
		        double tx, ty, denx, deny, t;
				bool bx = false;
				bool by = false;
				t = tx = ty = Double.NaN;
				denx = this.p2.x - this.p1.x;						// Calcola i denominatori
				deny = this.p2.y - this.p1.y;
				if(Math.Abs(denx) > Line2D.CoincidenceDistance)		// Calcola i parametri t del punto, con le x e con le y
					{												// e li usa solo se > cooincidence distance, per evitare
					bx = true;										// approssimazioni numeriche
					tx = (p.x - this.p1.x) / denx;	
					}
				if(Math.Abs(deny) > Line2D.CoincidenceDistance)
					{
					by = true;
					ty = (p.y - this.p1.y) / deny;
					}
				if( bx && by)										// Calcola t come media oppure...
					{
					t = (tx+ty)*0.5;
					}
				else
					{
					if(bx) t = tx;									// con il valore valido
					if(by) t = ty;
					}
				if( t == Double.NaN)								// Se no: eccezione (era this.valid)
					throw new Exception("Errore in public bool Line2D.Belongs(Point2D p)");
		        if((this.Point(t) - p).Mod() <= Line2D.coincidencedistance)		// Distanza tra p e intersezione calcolata.
		            {
					if(bInside)										// Se richiesta appartenenza, controlla.
						{
						if( (t >= 0.0) && (t <= 1.0))				// Se appartiene al segmento, esce con true
							return true;
						}
					else
						return true;								// Se non richiesta, esce con true
		            }
		        }
		    return false;
		    }
		/// <summary>
		/// ToString()
		/// </summary>
		/// <returns></returns>
		public override string ToString() 
			{
			StringBuilder str = new StringBuilder("[");
			str.Append(p1.x);
			str.Append(';');
			str.Append(p1.y);
			str.Append("]-[");
			str.Append(p2.x);
			str.Append(';');
			str.Append(p2.y);
			str.Append(']');
			return str.ToString();
			}
		/// <summary>
		/// Disegna
		/// </summary>
		/// <param name="dc"></param>
		/// <param name="fin"></param>
		/// <param name="penna"></param>
		public override void Plot(Graphics dc, Finestra fin, Pen penna)
			{
			if(IsValid)
				{
				Point start, end;
				start = fin.Get(this.pStart);
				end = fin.Get(this.pEnd);
				dc.DrawLine(penna,start,end);
				}
			}
		/// <summary>
		/// Aggiunge alla dsiplay list
		/// </summary>
		/// <param name="displaylist"></param>
		/// <param name="penna"></param>
		public override void Display(DisplayList displaylist, int penna)
			{
			if(IsValid)
				displaylist.Add(new DisplayListElement(this,penna));
			}
		#endregion
		}
	} // Fine namespace Fred68.Tools.Matematica
