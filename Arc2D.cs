using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Fred68.Tools.Matematica;
using Fred68.Tools.Grafica;

namespace Fred68.Tools.Matematica
	{
	/// <summary> Arco </summary>
	public class Arc2D : Tratto
		{
		#pragma warning disable 1591
		protected static int d2D = 2;
		public static int Dim2D { get { return d2D; } }
		protected static readonly double PI2 = 2.0 * Math.PI;									// 2 pigreco
		protected static readonly double epsilon = System.Double.Epsilon;						// Epsilon
		public static double Epsilon { get { return epsilon; } }
		protected static readonly double coincidencedistance = 1E-12;							// Distanza minima due punti distinti
		public static double CoincidenceDistance { get { return coincidencedistance; } }
		/// <summary>
		/// Tipo di arco per tre punti richiesto
		/// </summary>
		public enum TrePunti {Estremi_e_Centro, Tre_Punti};
		#region PROTECTED
		protected Point2D pCenter;							// Centro
		protected double radius;							// Versore
		protected bool bValid;
		protected Angolo alfaIni;							// Angolo iniziale (arco tracciato in senso orario...)					
		protected Angolo alfaFin;							// Angolo finale (...per distinguere l'arco interno dall'esterno)
		protected Point2D pIni;								// Punto iniziale e finale. Nota: punti non usati se arco nullo...
		protected Point2D pFin;								// ...(alfaIni=alfaFin=0.0) o circonferenza (alfaIni=alfaFin=2*PI)
		protected bool bCircle;								// flag che indica cerchio intero
		#endregion 
		#pragma warning restore 1591
		#region PROPRIETA
		/// <summary>
		/// Centro
		/// </summary>
		public Point2D Center								// Centro 
								{
								get { return pCenter; }
								set {
									pCenter = value;
									Validate();
									}
								}
		/// <summary>
		/// Raggio
		/// </summary>
		public double Radius								// Raggio 
								{
								get {return radius;}
								set	{
									radius = value;
									Validate();
									}
								}
		/// <summary>
		/// Valido
		/// </summary>
		public override bool IsValid						// Se e` valido 
							{
							get {return bValid;}
							}
		/// <summary>
		/// Se e` un cerchio
		/// </summary>
		public bool IsCircle								// Se e` un cerchio 
							{
							get {return bCircle;}
							set {
								bCircle = value;
								Validate();
								}
							}
		/// <summary>
		/// Punto iniziale
		/// </summary>
		public Point2D PIni									// Restituisce il punto iniziale... 
							{
							get {return pIni;}
							}
		/// <summary>
		/// Punto finale
		/// </summary>
		public Point2D PFin									// ...ed il punto finale 
							{
							get {return pFin;}
							}
		/// <summary>
		/// Angolo iniziale in radianti tra -PI e PI
		/// </summary>
		public double AlfaIni
			{
			get {return alfaIni;}
			}
		/// <summary>
		/// Angolo finale in radianti tra -PI e PI
		/// </summary>
		public double AlfaFin
			{
			get {return alfaFin;}
			}
		/// <summary>
		/// Punto iniziale
		/// </summary>
		public override Point2D pStart 
			{
			get {return pIni;}
			}
		/// <summary>
		/// Punto finale
		/// </summary>
		public override Point2D pEnd 
			{
			get {return pFin;}
			}
		/// <summary>
		/// Array con punto iniziale e finale
		/// </summary>
		public override Point2D[] P12 
			{
			get
				{
				Point2D[] ap = new Point2D[2];
				ap[0] = pIni;
				ap[1] = pFin;
				return ap;
				}		
			}
		#endregion
		#region COSTRUTTORI
		/// <summary>
		/// Arco
		/// </summary>
		public Arc2D() : base(typeof(Arc2D)) 
			{
			pCenter = null;
			radius = 0.0;
			alfaIni = alfaFin = 0.0;
			bCircle = false;
			pIni = null;
			pFin = null;
			bValid = false;
			}
		/// <summary>
		/// Arco
		/// </summary>
		/// <param name="center">Centro</param>
		/// <param name="radius">Raggio</param>
		/// <param name="alfaIni">Angolo iniziale</param>
		/// <param name="alfaFin">Angolo finale</param>
		public Arc2D(Point2D center, double radius, double alfaIni, double alfaFin) : base(typeof(Arc2D)) 
			{
			this.pCenter = center;
			this.radius = radius;
			this.alfaIni = alfaIni;
			this.alfaFin = alfaFin;
			pIni = null;
			pFin = null;
			Validate();
			}
		/// <summary>
		/// Cerchio
		/// </summary>
		/// <param name="center">Centro</param>
		/// <param name="radius">Raggio</param>
		public Arc2D(Point2D center, double radius) : base(typeof(Arc2D)) 
		    {
			this.pCenter = center;
			this.radius = radius;
			bCircle = true;
			pIni = null;
			pFin = null;
			Validate();
		    }
		/// <summary>
		/// Arco
		/// </summary>
		/// <param name="cx">x del centro</param>
		/// <param name="cy">y del centro</param>
		/// <param name="radius">Raggio</param>
		/// <param name="alfaIni">Angolo iniziale</param>
		/// <param name="alfaFin">Angolo finale</param>
		public Arc2D(double cx, double cy, double radius, double alfaIni, double alfaFin) : base(typeof(Arc2D)) 
			{
			this.pCenter = new Point2D(cx, cy);
			this.radius = radius;
			this.alfaIni = alfaIni;
			this.alfaFin = alfaFin;
			pIni = null;
			pFin = null;			
			Validate();
			}
		/// <summary>
		/// Cerchio
		/// </summary>
		/// <param name="cx">x del centro</param>
		/// <param name="cy">y del centro</param>
		/// <param name="radius">Raggio</param>
		public Arc2D(double cx, double cy, double radius) : base(typeof(Arc2D))	
			{
			this.pCenter = new Point2D(cx, cy);
			this.radius = radius;
			bCircle = true;
			pIni = null;
			pFin = null;
			Validate();
			}
		/// <summary>
		/// Arco per centro e due punti
		/// </summary>
		/// <param name="center">Centro</param>
		/// <param name="radius">Raggio (opzionale)</param>
		/// <param name="pIni">Punto iniziale</param>
		/// <param name="pFin">Punto finale</param>
		/// <param name="useRadius">true: usa il raggio, false: lo calcola in base ai punti</param>
		public Arc2D(Point2D center, double radius, Point2D pIni, Point2D pFin, bool useRadius) : base(typeof(Arc2D)) 
			{
			this.pCenter = center;
			Point2D dirIni = pIni - center;						// Direzioni dei punti
			Point2D dirFin = pFin - center;
			double rIni = dirIni.Mod();							// Calcola i moduli
			double rFin = dirFin.Mod();
			if(useRadius)
				{
				this.radius = radius;
				}
			else
				{
				this.radius = (rIni + rFin)*0.5;		// Calcola raggio medio
				}
			alfaIni = dirIni.Alfa();					// Calcola gli angoli
			alfaFin = dirFin.Alfa();
			dirIni.Normalize();		 					// Normalizza le direzioni
			dirFin.Normalize();
			this.pIni = center + this.radius * dirIni;		// Calcola i punti senza usare gli angoli, imprecisi
			this.pFin = center + this.radius * dirFin;
			Validate();
			}
		/// <summary>
		/// Cerchio per centro e un punto
		/// </summary>
		/// <param name="center">Centro</param>
		/// <param name="point">Punto sulla circonferenza</param>
		public Arc2D(Point2D center, Point2D point)	: this(center, Function2D.Distance(center, point))
			{}
		/// <summary>
		/// Arco per centro e due punti 
		/// </summary>
		/// <param name="center">Centro</param>
		/// <param name="pIni">Punto iniziale</param>
		/// <param name="pFin">Punto finale</param>
		public Arc2D(Point2D center, Point2D pIni, Point2D pFin) : this(center, 0.0, pIni, pFin, false)
		    {}
		/// <summary>
		/// Arco per punti estremi e centro oppure terzo punto.
		/// Mantiene invariati gli oggetti pIni e pFin
		/// </summary>
		/// <param name="pIni">punto iniziale (mantenuto invariato)</param>
		/// <param name="pFin">punto finale (mantenuto invariato)</param>
		/// <param name="pCenter_or_3dPoint">centro o terzo punti</param>
		/// <param name="trepunti">TrePunti.tipo</param>
		public Arc2D(Point2D pIni, Point2D pFin, Point2D pCenter_or_3dPoint, TrePunti trepunti)  : base(typeof(Arc2D))
			{
			if(trepunti == TrePunti.Estremi_e_Centro)
				{
				this.pIni = pIni;												// Imposta gli estremi...
				this.pFin = pFin;												// ...e non li cambia piu`
				Point2D mid = 0.5 * (pIni + pFin);								// Punto medio
				Point2D norm = (pFin - pIni).Normal();							// Normale
				double rIni = (pIni - pCenter_or_3dPoint).Mod();
				double rFin = (pFin - pCenter_or_3dPoint).Mod();
				double d12 = 0.5 * ((pFin - pIni).Mod());						// Meta` lunghezza
				this.radius = 0.5 * (rIni + rFin);								// Raggio (medio)
				double apo = Math.Sqrt( radius*radius - d12*d12 );				// Apotema
				this.pCenter = mid + (apo * Math.Sign(norm^(pCenter_or_3dPoint-mid))) * norm;	// Centro
				Point2D dirIni = pIni - this.pCenter;								// Direzioni dei punti
				Point2D dirFin = pFin - this.pCenter;
				alfaIni = dirIni.Alfa();										// Calcola gli angoli
				alfaFin = dirFin.Alfa();
				Validate();
				}
			else if(trepunti == TrePunti.Tre_Punti)
				{
				throw new Exception("Construttore non completamente implementato");
				}
			}
		#endregion
		#region FUNZIONI
		/// <summary>
		/// Verifica e ricalcola
		/// </summary>
		public override void Validate() 
			{
			if (radius <= Arc2D.Epsilon)					// Verifica radius > epsilon
				{
				bValid = false;
				return;
				}
			if(bCircle == false)								// Se non cerchio completo...
				{
				#warning Verificare se Angolo funziona correttamente in Arc2D.Validate()
				//alfaIni = Math.IEEERemainder(alfaIni, PI2);		// Normalizza tra -PI e +PI
				//alfaFin = Math.IEEERemainder(alfaFin, PI2);		// ORA SUPERFLUO, CON CLASSE Angolo
				if (pIni == null)								// Ricalcola pIni e pFin solo se nulli
					pIni = new Point2D(radius * Math.Cos(alfaIni), radius * Math.Sin(alfaIni));
				if(pFin == null)
					pFin = new Point2D(radius * Math.Cos(alfaFin), radius * Math.Sin(alfaFin));
				}
			bValid = true;
			}
		/// <summary>
		/// Angolo di un punto qualunque rispetto al centro
		/// </summary>
		/// <param name="p">Punto</param>
		/// <param name="alfa">Alfa (parametro out)</param>
		/// <returns>false se errore</returns>
		public bool Alfa(Point2D p, out double alfa) 
			{
			if (radius <= Arc2D.Epsilon)					// Verifica radius > epsilon
				{
				alfa = 0.0;
				return false;
				}
			Point2D dir = p - pCenter;						// Vettore dal centro al punto...
			alfa = dir.Alfa();								// ...e ancgolo di fase
			return true;
			}
		/// <summary>
		/// Punto appartenente all'arco, con angolo alfa
		/// oppure null se alfa non appartiene all'arco
		/// </summary>
		/// <param name="alfa">Angolo</param>
		/// <returns>Il punto</returns>
		public Point2D Point(double alfa) 
			{
			Point2D punto = null;				// Punto inizialmente null
			if(Belongs(alfa))					// Se alfa appartiene all'arco
				{
				Point2D versore = new Point2D(Math.Cos(alfa), Math.Sin(alfa));	// Calcola versore
				punto = Center + Radius * versore;	// Calcola il punto
				}
			return punto;
			}
		/// <summary>
		/// Verifica se l'angolo appartiene all'arco
		/// </summary>
		/// <param name="alfa">Angolo</param>
		/// <returns>true se appartiene</returns>
		public bool Belongs(double alfa) 
			{													// alfaIni e alfaFin compresi tra -PI e PI
			if (bCircle == true)	return true;				// Se e` un cerchio, appartiene di sicuro
			double alfan = Math.IEEERemainder(alfa, PI2);	// Normalizza alfa tra -PI e +PI
			if(alfaIni < alfaFin)								// Se l'arco non comprende -PI...
				{
				if((alfan >= alfaIni) && (alfan <= alfaFin))	// ...appartiene se > ini e < fin
					return true;
				else
					return false;
				}
			else												// Se l'arco comprende -PI...
				{
				if((alfan > alfaFin) && (alfan < alfaIni))		// non appartiene se > fin e < ini
					return false;
				else
					return true;
				}
			}
		/// <summary>
		/// Verifica se il punto appartiene all'arco
		/// </summary>
		/// <param name="p">Punto</param>
		/// <param name="bInside">true se richiede appartenenza</param>
		/// <returns>true se appartiene</returns>
		public override bool Belongs(Point2D p, bool bInside = false)	// Appartenenza del punto all'arco 
			{
			if( Math.Abs(Function2D.Distance(p, pCenter) - radius) <= Arc2D.coincidencedistance)	// Se a distanza radius dal centro
				{											
				double alfa;
				if (Alfa(p, out alfa))						// Calcola angolo del punto
					{										// Se non ci sono errori...
					if(bInside)								// Se richiesta appartenenza...
						{
						if(Belongs(alfa))					// ...controlla se l'angolo appartiene all'arco
							{
							return true;					// Se si`, esce con true
							}
						}
					else									// Se non richiesta appartenenza...
						{
						return true;						// esce con true
						}
					}
				}
			return false;
			}
		/// <summary>
		/// ToString()
		/// </summary>
		/// <returns></returns>
		public override string ToString()					// ToString() 
			{
			StringBuilder str = new StringBuilder("C[");
			str.Append(pCenter.x);
			str.Append(';');
			str.Append(pCenter.y);
			str.Append("]-R");
			str.Append(radius);
			if(!bCircle)
				{
				str.Append("-[");
				str.Append(pIni.ToString());
				str.Append("]>[");
				str.Append(pFin.ToString());
				str.Append("]");
				}
			return str.ToString();
			}
		/// <summary>
		/// Plot
		/// </summary>
		/// <param name="dc"></param>
		/// <param name="fin"></param>
		/// <param name="penna"></param>
		public override void Plot(Graphics dc, Finestra fin, Pen penna)
			{
			if(IsValid)
				{
				Point2D bassosx, altodx, diag_meta;
				float alfini, alffin, alfswp;

				diag_meta = new Point2D( this.Radius, this.Radius);
				bassosx = this.Center - diag_meta;
				altodx = this.Center + diag_meta;
				
				if(IsCircle)
					{
					Point bsx, adx;
					bsx = fin.Get(bassosx);
					adx = fin.Get(altodx);
					int xm,ym,xM,yM;
					xm = Math.Min(bsx.X, adx.X);
					xM = Math.Max(bsx.X, adx.X);
					ym = Math.Min(bsx.Y, adx.Y);
					yM = Math.Max(bsx.Y, adx.Y);

					Rectangle rct = new Rectangle(new Point(xm,ym), new Size(xM-xm,yM-ym));
					dc.DrawEllipse(penna,rct);
					}
				else
					{
					Point start, end, center, bsx, adx;
					start = fin.Get(this.pStart);
					end = fin.Get(this.pEnd);
					center = fin.Get( this.Center);
					bsx = fin.Get(bassosx);
					adx = fin.Get(altodx);
					int xm,ym,xM,yM;
					xm = Math.Min(bsx.X, adx.X);
					xM = Math.Max(bsx.X, adx.X);
					ym = Math.Min(bsx.Y, adx.Y);
					yM = Math.Max(bsx.Y, adx.Y);

					Rectangle rct = new Rectangle(new Point(xm,ym), new Size(xM-xm,yM-ym));

					// Cambiati segni e scambiati ini con fin...
					alffin = (float)(-this.AlfaIni*180/Math.PI);
					alfini = (float)(-this.AlfaFin*180/Math.PI);
				
					if(alfini < 0.0)
						alfini = 360.0F + alfini;
					if(alffin < 0.0)
						alffin = 360.0F + alffin;			// alfaini e alfafin corretti e concordi

					if(alffin >= alfini)					// calcolo sweep
						alfswp = alffin - alfini;
					else
						alfswp = 360.0F-alfini + alffin;

					dc.DrawArc(penna,rct,alfini,alfswp);
					}
				}
			}
		/// <summary>
		/// Aggiunge alla display list
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
	}
