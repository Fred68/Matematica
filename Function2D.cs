using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using Fred68.Tools.Matematica;
using Fred68.Tools.Grafica;

namespace Fred68.Tools.Matematica
	{
	/// <summary> Classe statica non istanziabile con molte funzioni di supporto </summary>
	public static class Function2D 
		{
		#pragma warning disable 1591
		private static readonly double epsilon = System.Double.Epsilon;							// Epsilon
		public static double Epsilon { get { return epsilon; } }
		private static readonly double coincidencedistance = 1E-12;								// Distanza minima due punti distinti
		public static double CoincidenceDistance { get { return coincidencedistance; } }
		#pragma warning restore 1591
		#region PUNTI NOTEVOLI
		/// <summary>
		/// Punto medio tra due punti
		/// </summary>
		/// <param name="p1">Primo punto</param>
		/// <param name="p2">Secondo punto</param>
		/// <returns></returns>
		public static Point2D Midpoint(Point2D p1, Point2D p2) 
			{
			return 0.5 * (p1 + p2);
			}
		#endregion
		#region DISTANZE e VARIE
		/// <summary>
		/// Distanza tra due punti
		/// </summary>
		/// <param name="p1">Primo punto</param>
		/// <param name="p2">Secondo punto</param>
		/// <returns></returns>
		public static double Distance(Point2D p1, Point2D p2) 
			{
			return (p2 - p1).Mod();
			}
		/// <summary>
		/// Distanza punto - retta
		/// </summary>
		/// <param name="p">Punto</param>
		/// <param name="l">Linea</param>
		/// <returns></returns>
		public static double Distance(Point2D p, Line2D l) 
			{													// Scalare tra versore normale 
			return (p - l.P2) ^ ((l.Vector()).Normal());		// e segmento Po - P1 o Po - P2
			}
		/// <summary>
		/// Distanza punto - tratto generico
		/// </summary>
		/// <param name="p">Punto</param>
		/// <param name="te">Tratto</param>
		/// <returns></returns>
		public static double Distance(Point2D p, Tratto te)
		    {
		    Type tp;
		    tp = te.GetType();
		    if(tp == typeof(Line2D))
		        {
		        return Distance( p, (Line2D) te);
		        }		
		    if(tp == typeof(Arc2D))
		        {
				throw new Exception("Tipo non implementato");		
		        // return Distance(p, (Arc2D) te);
		        }		
		    throw new Exception("Tipo non implementato");
		    }
		/// <summary>
		/// Controlla se due tratti sono connessi
		/// </summary>
		/// <param name="t1"></param>
		/// <param name="t2"></param>
		/// <returns></returns>
		public static bool AreConnected(Tratto t1, Tratto t2)
			{
			bool conn = false;
			int i,j;
			for(i=0; i<2; i++)
				for(j=0; j<2; j++)
					{
					if ( Function2D.Distance(t1.P12[i],t2.P12[j]) <= Function2D.CoincidenceDistance)
						{
						conn = true;			// Imposta flag 
						i=j=2;					// e uscita dai cicli
						}
					}
			return conn;
			}
		#endregion		
		#region INTERSEZIONI
		/// <summary>
		/// Intersezioni tra due linee
		/// </summary>
		/// <param name="l1">Prima linea</param>
		/// <param name="l2">Seconda linea</param>
		/// <param name="bCheckInside1">Richiesta intersezione interna alla prima linea</param>
		/// <param name="bCheckInside2">Richiesta intersezione interna alla secondalinea</param>
		/// <returns></returns>
		public static List<Intersection> Intersect(	Line2D l1, Line2D l2, bool bCheckInside1 = false, bool bCheckInside2 = false) 
			{
			double t1, t2;
			List<Intersection> intersezioni = new List<Intersection>();
			LinearSys ls = new LinearSys();			// Sistema di equazioni lineari
			Matrix a = new Matrix(2,2);				// Imposta matrice e vettore
			a.Set(	0,	0, l1.P2.x - l1.P1.x );
			a.Set(	0,	1, l2.P1.x - l2.P2.x );
			a.Set(	1,	0, l1.P2.y - l1.P1.y );
			a.Set(	1,	1, l2.P1.y - l2.P2.y );
			Matrix b = new Matrix(2, 1);
			b.Set( 0, 0, l2.P1.x - l1.P1.x );
			b.Set( 1, 0, l2.P1.y - l1.P1.y );
			Matrix x = ls.Solve(a , b);				// Risolve
			if(!x.IsNull)
				{
				t1 = x.Get(0, 0);					// Estrae i parametri delle due rette
				t2 = x.Get(1, 0);
				Point2D i1, i2;
				i1 = l1.P1 + t1 * (l1.P2 - l1.P1);	// Calcola i due punti
				i2 = l2.P1 + t2 * (l2.P2 - l2.P1);
//				if(Function2D.Distance(i1, i2) <= Function2D.CoincidenceDistance) {...}
				Point2D pint = Function2D.Midpoint(i1, i2);				// Calcola il punto (medio) di intersezione, in ogni caso...
				bool add = true;										// anche se distanza > coincidence distance.
				if(bCheckInside1)										// Se richiesto punto interno verifica...
					{
					if( (t1 < 0.0) || (t1 > 1.0) )						// Se t1 indica che e` esterno, imposta a false
						add = false;
					}
				if(bCheckInside2)										// Se richiesto punto interno verifica...
					{
					if( (t2 < 0.0) || (t2 > 1.0) )						// Se t1 indica che e` esterno, imposta a false
						add = false;
					}
				if(add)
					intersezioni.Add(new Intersection(pint, t1, t2, l1, l2));	// Aggiunge intersezione con parametri alla lista
				}
			return intersezioni;					// Restituisce la lista
			}
		/// <summary>
		/// Intersezioni tra linea ed arco
		/// </summary>
		/// <param name="l1">Linea</param>
		/// <param name="a2">Arco</param>
		/// <param name="bCheckInside1">Richiesta intersezione interna alla linea</param>
		/// <param name="bCheckInside2">Richiesta intersezione interna all'arco</param>
		/// <returns></returns>
		public static List<Intersection> Intersect(	Line2D l1, Arc2D a2, bool bCheckInside1 = false, bool bCheckInside2 = false) 
			{
			double[] t = new double[2];
			double[] a = new double[2];
			Point2D[] p = new Point2D[2];
			bool[] ok = new bool[2];												// soluzione trovata e valida
			bool[] tg = new bool[2];												// intersezione doppia
			List<Intersection> intersezioni = new List<Intersection>();

			// P = P1 + t (P2 - P1)															equazione della retta del segmento
			// P = C + r (cos a, sin a) oppure												equazione della circonferenza
			// || P - C ||^2 = r^2															altra equazione della circonferenza
			// || P1 - C + t (P2 - P1) ||^2 = r^2											Intersezione: punto su entrambi
			// A = P1 - C; B = P2 - P1														Sostituzioni
			// || V || = V.x^2 + V.y^2 = V ^ V (scalare)									Definizione di modulo
			// || A + t B ||^2 = r^2														Intersezione, sostituendo...
			// (A.x + t B.x)^2 + (A.y + t B.y)^2 - r^2 = 0									...
			// A.x^2 + t^2 B.x^2 + 2 t A.x B.x + A.y^2 + t^2 B.y^2 + 2 t A.y B.y - r^2 = 0	...
			// A.x^2 + A.y^2 + t^2 ( B.x^2 + B.y^2) + 2 t ( A.y B.y + A.x B.x) - r^2 = 0	...
			// t^2(B^B) + 2 t (A^B) + A^A - r^2 = 0											Equazione di secondo grado da risolvere
			// a=B^B	2b=2*A^B	c=A^A-r^2	in	a*t^2 + 2*b*t + c = 0					Coefficienti
			// t12 = [-2b +- sqr( (2b)^2 - 4ac) ] / 2a = [-2b +- 2sqr( b^2 - ac) ] / 2a		Formula completa...
			// t1 = [-b +- sqr(b^2-ac)]/a													...e ridotta

			Point2D A = l1.P1 - a2.Center;
			Point2D B = l1.P2 - l1.P1;
			double r = a2.Radius;
			ok[0] = ok[1] = false;								// Imposta i flag
			tg[0] = tg[1] = false;

			double aEq, bEq, cEq, dEq24, dEq2;					// Calcola con formula ridotta ERRORE NELLA FORMULA ???
			aEq = B^B;
			bEq = A^B;
			cEq = (A^A) - r*r;			
			dEq24 = bEq*bEq - aEq*cEq;

			if(dEq24 >= 0.0)									// Cerca le soluzioni, memorizza valori ed imposta i flag
				{												// Se ci sono soluzioni
				if(Math.Abs(dEq24) < Function2D.epsilon)		// Delta = 0, una soluzione
					{
					t[0] = -bEq / aEq;
					p[0] = l1.P1 + t[0] * (l1.P2 - l1.P1);
					if(a2.Alfa(p[0], out a[0]))					// Calcola alfa dell'arco e imposta flag della soluzione
						{
						ok[0] = true;
						tg[0] = true;							// tangente, soluzione doppia
						}
					}
				else											// Delta > 0, due soluzioni
					{
					dEq2 = Math.Sqrt(dEq24);					// Radice di delta	
					t[0] = (-bEq - dEq2) / aEq;
					t[1] = (-bEq + dEq2) / aEq;
					p[0] = l1.P1 + t[0] * (l1.P2 - l1.P1);
					p[1] = l1.P1 + t[1] * (l1.P2 - l1.P1);
					if(a2.Alfa(p[0], out a[0]))					// Calcola alfa e flag delle due soluzioni
						ok[0] = true;
					if(a2.Alfa(p[1], out a[1]))
						ok[1] = true;
					}
				}
			for(int i=0; i<2; i++)								// Verifica, se richieste, le appartenenze a segmento e arco
				{
				if(ok[i])										// Esamina, se c'e', la soluzione
					{
					if(bCheckInside1)							// Se richiesto punto interno verifica...
						{
						if( (t[i] < 0.0) || (t[i] > 1.0) )		// Se t trovato indica che e` esterno, imposta a false
							ok[i] = false;
						}
					if(bCheckInside2)							// Idem per l'arco...
						{
						if(!a2.Belongs(a[i]))
							ok[i] = false;
						}
					}
				}
			for(int i=0; i<2; i++)								// Riesamina le soluzione
				{
				if(ok[i])										// Se trovata, aggiunge intersezione alla lista
					{
					intersezioni.Add(new Intersection(p[i], t[i], a[i], l1, a2, false, tg[i]));
					}
				}
			return intersezioni;								// Restituisce il rif. alla lista
			}
		/// <summary>
		/// Intersezione tra linea e tratto generico
		/// </summary>
		/// <param name="l1">Linea</param>
		/// <param name="te">Tratto</param>
		/// <param name="bCheckInside1">Richiesta intersezione interna alla linea</param>
		/// <param name="bCheckInside2">Richiesta intersezione interna al tratto</param>
		/// <returns></returns>
		public static List<Intersection> Intersect( Line2D l1, Tratto te, bool bCheckInside1 = false, bool bCheckInside2 = false)
		    {
			Type tp;
			tp = te.GetType();
			if(tp == typeof(Line2D))
			    {
			    return Intersect( l1, (Line2D)te, bCheckInside1 , bCheckInside2 );
			    }		
			if(tp == typeof(Arc2D))
			    {
			    return Intersect( l1, (Arc2D)te, bCheckInside1 , bCheckInside2 );
			    }		
		    throw new Exception("Tipo non implementato");
		    }

		#endregion
		#region PUNTI PIU` VICINI
		/// <summary>
		/// Punto piu` vicino a p, tra i punti di una lista
		/// </summary>
		/// <param name="p">Punto p</param>
		/// <param name="pl">Lista di punti</param>
		/// <param name="nearest">Punto trovato (parametro out)</param>
		/// <param name="distance">Distanza (parametro out)</param>
		/// <returns>true se trova il punto</returns>
		public static bool		Nearest( Point2D p, List<Point2D> pl, out Point2D nearest, out double distance) 
			{
			double dTemp;
			bool bFound = false;
			distance = double.MaxValue;				
			nearest = new Point2D();
			foreach(Point2D lp in pl)
				{
				dTemp = Function2D.Distance(lp, p);
				if (dTemp < distance)
					{
					nearest = lp;
					distance = dTemp;
					bFound = true;
					}
				}
			return bFound;
			}
		/// <summary>
		/// Punto piu` vicino a p, tra i punti di una lista
		/// </summary>
		/// <param name="p">Punto p</param>
		/// <param name="pl">Lista</param>
		/// <returns>Il punto trovato, se no null</returns>
		public static Point2D	Nearest( Point2D p, List<Point2D> pl) 
			{
			Point2D nearest;
			double distance;
			if( ! Nearest(p, pl, out nearest, out distance))			// Cerca il punto
				nearest = null;											// Se non trova: imposta a null
			return nearest;
			}
		/// <summary>
		/// Punto piu` vicino a p, su una linea
		/// </summary>
		/// <param name="p">Punto p</param>
		/// <param name="l">Linea</param>
		/// <param name="nearest">Punto trovato (parametro out)</param>
		/// <param name="distance">Distanza (parametro out)</param>
		/// <param name="bInside">true se richiesta intersezione interna alla linea</param>
		/// <returns>true se trovato</returns>
		public static bool		Nearest( Point2D p, Line2D l, out Point2D nearest, out double distance, bool bInside = false) 
			{
			bool bFound = false;
			nearest = new Point2D();
			distance = double.MaxValue;
			Point2D projection = new Point2D();
			List<Point2D> plist = new List<Point2D>();
			if(Function2D.Projection(p, l, out projection, bInside))		// Calcola la proiezione di p su l usando bInside
				{															// Se la trova, la aggiunge alla lista
				plist.Add(projection);
				}
			if (bInside)													// Se richiesto bInside, cioe` solo punti interni...
				{															// al segmento, ne aggiunge anche gli estremi alla lista
				plist.Add(l.P1);											// (se bInside e` false, la prioezione e` comunque
				plist.Add(l.P2);											// il piu` vicino
				}
			bFound = Function2D.Nearest(p, plist, out nearest, out distance);	// Cerca il punto piu` vicino nella lista
			return bFound;
			}
		/// <summary>
		/// Punto piu` vicino a p, su una linea
		/// </summary>
		/// <param name="p">Punto p</param>
		/// <param name="l">Linea</param>
		/// <param name="bInside">true se richiesta intersezione interna alla linea</param>
		/// <returns>Il punto trovato, se no null</returns>
		public static Point2D	Nearest( Point2D p, Line2D l, bool bInside = false) 
			{
			Point2D nearest;
			double distance;
			if( ! Nearest(p, l, out nearest, out distance, bInside))	// Cerca il punto.
				nearest = null;											// Se non trova: imposta a null
			return nearest;
			}
		/// <summary>
		/// Punto piu` vicino a p, su un arco
		/// </summary>
		/// <param name="p">Punto p</param>
		/// <param name="a">Arco</param>
		/// <param name="nearest">Punto trovato (parametro out)</param>
		/// <param name="distance">Distanza (parametro out)</param>
		/// <param name="bInside">true se richiesta intersezione interna all'arco</param>
		/// <returns>true se trovato</returns>
		public static bool		Nearest( Point2D p, Arc2D a, out Point2D nearest, out double distance, bool bInside = false) 
			{
			bool bFound = false;
			nearest = new Point2D();
			distance = double.MaxValue;
			Point2D projection = new Point2D();
			List<Point2D> plist = new List<Point2D>();
			if(Function2D.Projection(p, a, out projection, bInside))		// Calcola le proiezioni di p su a usando bInside
				{															// Se la trova, la aggiunge alla lista
				plist.Add(projection);
				}
			if (bInside)													// Se richiesto bInside, cioe` solo punti interni...
				{															// al segmento, ne aggiunge anche gli estremi alla lista
				plist.Add(a.PIni);											// (se bInside e` false, la prioezione e` comunque
				plist.Add(a.PFin);											// il piu` vicino
				}
			bFound = Function2D.Nearest(p, plist, out nearest, out distance);	// Cerca il punto piu` vicino nella lista
			return bFound;
			}
		/// <summary>
		/// Punto piu` vicino a p, su un arco
		/// </summary>
		/// <param name="p">Punto p</param>
		/// <param name="a">Arco</param>
		/// <param name="bInside">true se richiesta intersezione interna all'arco</param>
		/// <returns>Il punto trovato, se no null</returns>
		public static Point2D	Nearest( Point2D p, Arc2D a, bool bInside = false) 
			{
			Point2D nearest;
			double distance;
			if( ! Nearest(p, a, out nearest, out distance, bInside))	// Cerca il punto
				nearest = null;											// Se non trova: imposta a null
			return nearest;
			}
		/// <summary>
		/// Punto piu` vicino, su un tratto generico
		/// </summary>
		/// <param name="p">Punto</param>
		/// <param name="te">Tratto</param>
		/// <param name="nearest">Punto trovato (parametro out)</param>
		/// <param name="distance">Distanza (parametro out)</param>
		/// <param name="bInside">true se richiesta intersezione interna al tratto</param>
		/// <returns>true se trovato</returns>
		public static bool		Nearest( Point2D p, Tratto te, out Point2D nearest, out double distance, bool bInside = false)
			{
			Type tp;
			tp = te.GetType();
			if(tp == typeof(Line2D))
			    {
			    return Nearest( p, (Line2D)te, out nearest, out distance, bInside);
			    }		
			if(tp == typeof(Arc2D))
			    {
			    return Nearest(p, (Arc2D)te, out nearest, out distance, bInside);
			    }		
		    throw new Exception("Tipo non implementato");
			}
		/// <summary>
		/// Punto piu` vicino, su un tratto generico
		/// </summary>
		/// <param name="p">Punto</param>
		/// <param name="te">Tratto</param>
		/// <param name="bInside">true se richiesta intersezione interna al tratto</param>
		/// <returns>Il punto trovato, se no null</returns>
		public static Point2D	Nearest( Point2D p, Tratto te, bool bInside = false)
			{
			Type tp;
			tp = te.GetType();
			if(tp == typeof(Line2D))
			    {
			    return Nearest( p, (Line2D)te, bInside);
			    }		
			if(tp == typeof(Arc2D))
			    {
			    return Nearest( p, (Arc2D)te, bInside);
			    }		
		    throw new Exception("Tipo non implementato");
			}

		#endregion
		#region PROIEZIONI 
		/// <summary>
		/// Proiezione di un punto su una linea
		/// </summary>
		/// <param name="p">Punto</param>
		/// <param name="l">Linea</param>
		/// <param name="projection">Proiezione (parametro out)</param>
		/// <param name="bInside">true se richiesta appartenenza proiezione all'interno della linea</param>
		/// <returns>true se trovata</returns>
		public static bool		Projection( Point2D p, Line2D l, out Point2D projection, bool bInside = false) 
			{
			Point2D tmp = l.Vector().Normal();
			Line2D prLine = new Line2D(p, l.Vector().Normal(), true);	// Calcola normale al segmento passante per p
			List<Intersection> lint = Function2D.Intersect(l, prLine, bInside, false);	// Interseca linea e normale, chiede eventuale appartenenza a linea
			if(lint.Count > 0)											// Se trovata almeno una intersezione
				{
				projection = (lint[0]).p;								// Estrae il punto di intersezione dal primo elemento della lista
				return true;
				}
			projection = new Point2D();									// Se non trova intersezione o se bInside e fuori dal segmento
			return false;												// restituisce false + punto vuoto
			}
		/// <summary>
		/// Proiezione di un punto su una linea
		/// </summary>
		/// <param name="p">Punto</param>
		/// <param name="l">Linea</param>
		/// <param name="bInside">true se richiesta appartenenza proiezione all'interno della linea</param>
		/// <returns>La proiezione</returns>
		public static Point2D	Projection( Point2D p, Line2D l, bool bInside = false) 
			{
			Point2D proj;
			if( ! Projection( p, l, out proj, bInside))		// Cerca il punto
				proj = null;								// Se non trova: imposta a null
			return proj;
			}
		/// <summary>
		/// Proiezione di un punto su un arco
		/// </summary>
		/// <param name="p">Punto</param>
		/// <param name="a">Arco</param>
		/// <param name="projection">Proiezione (parametro out)</param>
		/// <param name="bInside">true se richiesta appartenenza proiezione all'interno dell'arco</param>
		/// <returns>true se trovata</returns>
		public static bool		Projection( Point2D p, Arc2D a, out Point2D projection, bool bInside = false) 
			{
			Point2D proj = null;										// La proiezione, se trovata
			Point2D dir = p - a.Center;									// Calcola il vettore dal centro al punto
			if(dir.Normalize())											// Lo normalizza. Se nessun errore...
				{
				Point2D proj1 = a.Center + a.Radius * dir;				// Calcola le due proiezioni sul cerchio.
				Point2D proj2 = a.Center - a.Radius * dir;				
				double d1 = Function2D.Distance(proj1, p);				// e le distanze tra punto e proiezione
				double d2 = Function2D.Distance(proj2, p);
				bool app1, app2;										// Appartenenza all'arco delle due proiezione
				app1 = app2 = true;
				if( bInside == true)									// Verifica le appartenenze all'arco
					{
					double alfa;
					if(a.Alfa(proj1, out alfa))							// calcola l'angolo del primo e del secondo punto proiettato
						{												// scarta, se non appartiene all'arco
						if(!a.Belongs(alfa))		app1 = false;
						}
					if(a.Alfa(proj2, out alfa))
						{
						if(!a.Belongs(alfa))		app2 = false;
						}
					}													
				if( (app1==true) && (app2==true) )						// Se entrambi appartengono all'arco
					{													// scarta quello con distanza maggiore
					if(d1 >= d2)
						app1 = false;
					else
						app2 = false;
					}
				if(app1)												// Imposta proj con la proiezione valida...
					proj = proj1;										// ...se c'e`
				else if(app2)
					proj = proj2;
				}
			if(proj != null)											// Se trovata proiezione, esce con true...
				{
				projection = proj;
				return true;
				}
			projection = new Point2D();									
			return false;												// ...se non trovata, esce con false + punto vuoto.
			}
		/// <summary>
		/// Proiezione di un punto su un arco
		/// </summary>
		/// <param name="p">Punto</param>
		/// <param name="a">Arco</param>
		/// <param name="bInside">true se richiesta appartenenza proiezione all'interno della linea</param>
		/// <returns>La proiezione</returns>
		public static Point2D	Projection( Point2D p, Arc2D a, bool bInside = false) 
			{
			Point2D proj;
			if( ! Projection( p, a, out proj, bInside))		// Cerca il punto
				proj = null;								// Se non trova: imposta a null
			return proj;
			}
		/// <summary>
		/// Proiezione di un punto su un tratto generico
		/// </summary>
		/// <param name="p">Punto</param>
		/// <param name="te">Tratto</param>
		/// <param name="projection">Proiezione (parametro out)</param>
		/// <param name="bInside">true se richiesta appartenenza proiezione all'interno del tratto</param>
		/// <returns></returns>
		public static bool		Projection( Point2D p, Tratto te, out Point2D projection, bool bInside = false)
			{
			Type tp;
			tp = te.GetType();
			if(tp == typeof(Line2D))
			    {
			    return Projection( p, (Line2D) te, out projection, bInside);
			    }		
			if(tp == typeof(Arc2D))
			    {
			    return Projection( p, (Arc2D) te, out projection, bInside);
			    }		
		    throw new Exception("Tipo non implementato");
			}
		/// <summary>
		/// Proiezione di un punto su un tratto generico
		/// </summary>
		/// <param name="p">Punto</param>
		/// <param name="te">Tratto</param>
		/// <param name="bInside">true se richiesta appartenenza proiezione all'interno del tratto</param>
		/// <returns>La proiezione</returns>
		public static Point2D	Projection( Point2D p, Tratto te, bool bInside = false)
			{
			Type tp;
			tp = te.GetType();
			if(tp == typeof(Line2D))
			    {
			    return Projection( p, (Line2D) te, bInside);
			    }		
			if(tp == typeof(Arc2D))
			    {
			    return Projection( p, (Arc2D) te, bInside);
			    }		
		    throw new Exception("Tipo non implementato");
			}
		#endregion
		#region NORMALI
		/// <summary>
		/// Linea (modulo unitario) per il punto p normale ad l
		/// </summary>
		/// <param name="p">Punto p</param>
		/// <param name="l">Linea l</param>
		/// <param name="vOut">Linea normale (parametro out), passante per p</param>
		/// <param name="ext">Punto esterno opzionale per definire il verso</param>
		/// <returns>true se trovato</returns>
		public static bool		LineOut( Point2D p, Line2D l, out Line2D vOut, Point2D ext = null) 
			{
			bool prf;
			Point2D pr;
			vOut = null;
			prf = Projection(p, l, out pr, true);			// Proiezione su curva (appartenente)
			if(prf)											// Se trovata...
				{
				Point2D normale = l.Vector().Normal();		// Normale per il punto di proiezione
				if(ext != null)
					{
					double scalare = (ext-pr) ^ normale;	// Scalare tra vettore uscente e normale calcolata
					if(scalare < 0)
						normale = -normale;					// Inverte
					}
				vOut = new Line2D(pr, normale, true);		// Linea uscente da pr, lunghezza unitaria
				}
			return prf;										// restituisce false se errore
			}
		/// <summary>
		/// Versore (modulo unitario) per il punto p normale ad l
		/// </summary>
		/// <param name="p">Punto p</param>
		/// <param name="l">Linea l</param>
		/// <param name="vOut">Versore (parametro out)</param>
		/// <param name="ext">Punto esterno opzionale per definire il verso</param>
		/// <returns>true se trovato</returns>
		public static bool		VersorOut( Point2D p, Line2D l, out Point2D vOut, Point2D ext = null) 
			{
			bool prf;
			Point2D pr;
			vOut = null;
			prf = Projection(p, l, out pr, true);			// Proiezione su curva (appartenente)
			if(prf)											// Se trovata...
				{
				Point2D normale = l.Vector().Normal();		// Normale per il punto di proiezione
				if(ext != null)
					{
					double scalare = (ext-pr) ^ normale;	// Scalare tra vettore uscente e normale calcolata
					if(scalare < 0)
						normale = -normale;					// Inverte
					}
				vOut = normale;								// Versore da pr, lunghezza unitaria
				}
			return prf;										// restituisce false se errore
			}
		/// <summary>
		/// Linea (modulo unitario) per il punto p normale ad l
		/// </summary>
		/// <param name="p">Punto p</param>
		/// <param name="l">Linea l</param>
		/// <param name="ext">Punto esterno opzionale per definire il verso</param>
		/// <returns>La linea normale o null se non trovata </returns>
		public static Line2D	LineOut( Point2D p, Line2D l, Point2D ext = null) 
			{
			Line2D vOut;
			if( ! LineOut( p, l, out vOut, ext))
				vOut = null;
			return vOut;
			}
		/// <summary>
		/// Versore (modulo unitario) per il punto p normale ad l
		/// </summary>
		/// <param name="p">Punto p</param>
		/// <param name="l">Linea l</param>
		/// <param name="ext">Punto esterno opzionale per definire il verso</param>
		/// <returns>Il versore normale o null se non trovato</returns>
		public static Point2D	VersorOut( Point2D p, Line2D l, Point2D ext = null) 
		    {
		    Point2D vOut;
		    if( ! VersorOut( p, l, out vOut, ext))
		        vOut = null;
		    return vOut;
		    }
		/// <summary>
		/// Linea (modulo unitario) per il punto p normale ad un arco
		/// </summary>
		/// <param name="p">Punto p</param>
		/// <param name="a">Arco</param>
		/// <param name="vOut">Linea normale (parametro out)</param>
		/// <param name="ext">Punto esterno opzionale per definire il verso</param>
		/// <returns>true se trovato</returns>
		public static bool		LineOut( Point2D p, Arc2D a, out Line2D vOut, Point2D ext = null) 
			{
			bool prf;
			Point2D pr;
			vOut = null;
			prf = Projection(p, a, out pr, true);			// Proiezione su curva (appartenente)
			if(prf)											// Se trovata...
				{
				Point2D normale = (pr - a.Center);			// Normale per il punto di proiezione
				bool test = normale.Normalize();			// Normalizza modulo (false se fallisce)
				if(ext != null)
					{
					double scalare = (ext-pr) ^ normale;	// Scalare tra vettore uscente e normale calcolata
					if(scalare < 0)
						normale = -normale;					// Inverte
					}
				if(test)									// controllo extra
					vOut = new Line2D(pr, normale, true);	// Linea uscente da pr, lunghezza unitaria
				}
			return prf;										// restituisce false se errore
			}
		/// <summary>
		/// Versore (modulo unitario) per il punto p normale ad un arco
		/// </summary>
		/// <param name="p">Punto p</param>
		/// <param name="a">Arco</param>
		/// <param name="vOut">Versore normale (parametro out)</param>
		/// <param name="ext">Punto esterno opzionale per definire il verso</param>
		/// <returns>true se trovato</returns>
		public static bool		VersorOut( Point2D p, Arc2D a, out Point2D vOut, Point2D ext = null) 
			{
			bool prf;
			Point2D pr;
			vOut = null;
			prf = Projection(p, a, out pr, true);			// Proiezione su curva (appartenente)
			if(prf)											// Se trovata...
				{
				Point2D normale = (pr - a.Center);			// Normale per il punto di proiezione
				bool test = normale.Normalize();			// Normalizza modulo (false se fallisce)
				if(ext != null)
					{
					double scalare = (ext-pr) ^ normale;	// Scalare tra vettore uscente e normale calcolata
					if(scalare < 0)
						normale = -normale;					// Inverte
					}
				if(test)									// controllo extra
					vOut = normale;	// Linea uscente da pr, lunghezza unitaria
				}
			return prf;										// restituisce false se errore
			}	
		/// <summary>
		/// Linea (modulo unitario) per il punto p normale ad un arco
		/// </summary>
		/// <param name="p">Punto p</param>
		/// <param name="a">Arco</param>
		/// <param name="ext">Punto esterno opzionale per definire il verso</param>
		/// <returns>La linea normale o null se non trovata</returns>
		public static Line2D	LineOut( Point2D p, Arc2D a, Point2D ext = null) 
			{
			Line2D vOut;
			if( ! LineOut( p, a, out vOut, ext))
				vOut = null;
			return vOut;
			}
		/// <summary>
		/// Versore (modulo unitario) per il punto p normale ad un arco
		/// </summary>
		/// <param name="p">Punto p</param>
		/// <param name="a">Arco</param>
		/// <param name="ext">Punto esterno opzionale per definire il verso</param>
		/// <returns>Il versore normale o null se non trovato</returns>
		public static Point2D	VersorOut( Point2D p, Arc2D a, Point2D ext = null) 
		    {
		    Point2D vOut;
		    if( ! VersorOut( p, a, out vOut, ext))
		        vOut = null;
		    return vOut;
		    }
		/// <summary>
		/// Linea uscente
		/// </summary>
		/// <param name="p">Punto sul tratto</param>
		/// <param name="te">Tratto</param>
		/// <param name="vOut">Versore uscente (parametro out)</param>
		/// <param name="ext">Punto esterno opzionale</param>
		/// <returns>bool</returns>
		public static bool		LineOut( Point2D p, Tratto te, out Line2D vOut, Point2D ext = null)
			{
			Type tp;
			tp = te.GetType();
			if(tp == typeof(Line2D))
			    {
			    return LineOut( p, (Line2D) te, out vOut, ext);
			    }		
			if(tp == typeof(Arc2D))
			    {
			    return LineOut( p, (Arc2D) te, out vOut, ext);
			    }		
		    throw new Exception("Tipo non implementato");
			}
		
		/// <summary>
		/// Versore uscente
		/// </summary>
		/// <param name="p">Punto sul tratto</param>
		/// <param name="te">Tratto</param>
		/// <param name="vOut">Versore uscente (parametro out)</param>
		/// <param name="ext">Punto esterno opzionale</param>
		/// <returns>bool</returns>
		public static bool		VersorOut( Point2D p, Tratto te, out Point2D vOut, Point2D ext = null)
			{
			Type tp;
			tp = te.GetType();
			if(tp == typeof(Line2D))
			    {
			    return VersorOut( p, (Line2D) te, out vOut, ext);
			    }		
			if(tp == typeof(Arc2D))
			    {
			    return VersorOut( p, (Arc2D) te, out vOut, ext);
			    }		
		    throw new Exception("Tipo non implementato");

			}
		/// <summary>
		/// Linea uscente
		/// </summary>
		/// <param name="p">Punto sul tratto</param>
		/// <param name="te">Tratto</param>
		/// <param name="ext">Punto esterno opzionale</param>
		/// <returns></returns>
		public static Line2D	LineOut( Point2D p, Tratto te, Point2D ext = null)
			{
			Type tp;
			tp = te.GetType();
			if(tp == typeof(Line2D))
			    {
			    return LineOut( p, (Line2D) te, ext);
			    }		
			if(tp == typeof(Arc2D))
			    {
			    return LineOut( p, (Arc2D) te, ext);
			    }		
		    throw new Exception("Tipo non implementato");
			}
		/// <summary>
		/// Versore uscente
		/// </summary>
		/// <param name="p">Punto sul tratto</param>
		/// <param name="te">Tratto</param>
		/// <param name="ext">Punto esterno opzionale</param>
		/// <returns></returns>
		public static Point2D	VersorOut( Point2D p, Tratto te, Point2D ext = null)
			{
			Type tp;
			tp = te.GetType();
			if(tp == typeof(Line2D))
			    {
			    return VersorOut( p, (Line2D) te, ext);
			    }		
			if(tp == typeof(Arc2D))
			    {
			    return VersorOut( p, (Arc2D) te, ext);
			    }		
		    throw new Exception("Tipo non implementato");
			}
		#endregion
		#region INTERPOLAZIONE LINEARE
		/// <summary>
		/// Ordina lista punti secondo x
		/// </summary>
		/// <param name="punti"></param>
		public static void OrdinaInX(List<Point2D> punti)
			{
			punti.Sort(delegate(Point2D p1, Point2D p2)
							{
							if(p1.x > p2.x)
								return 1;
							else
								return -1;
							}
						);
			return;
			}
		/// <summary>
		/// Interpolazione lineare
		/// Se la lista contiene un solo valore, lo restituisce comunque
		/// Se no interpola solo tra il primo e l'ultimo punto.
		/// </summary>
		/// <param name="punti">Lista punti ordinata secondo x</param>
		/// <param name="x">Valore x da interpolare</param>
		/// <returns>Valore interpolato linearmente oppure NaN se fuori limiti</returns>
		public static double InterpolazioneLineare(List<Point2D> punti, double x)
			{
			if(punti.Count == 1)				// Se un solo valore, restiruisce la y ed esce
				return punti[0].y;
			if(punti.Count == 0)				// Se nessuno, restiruisce NaN
				return Double.NaN;
			int imin, imax;						// Percorre la lista e cerca i valori x precedente e successivo
			imin = imax = -1;
			double xmin, xmax;
			xmin = Double.MinValue;
			xmax = Double.MaxValue;
			for(int i=0; i<punti.Count; i++)
				{
				double px = punti[i].x;
				if( (px >= x) && (px < xmax) )
					{
					imax = i;
					xmax = px;
					}
				if( (px <= x) && (px > xmin) )
					{
					imin = i;
					xmin = px;
					}
				}
			if( (imin==-1) || (imax==-1))		// Punto esterno all'intervallo
				return Double.NaN;
			double t;							// Calcola la posizione di x
			if(xmax-xmin <= Double.Epsilon)
				t = 0.5;
			else
				t = (x-xmin)/(xmax-xmin);
			Line2D l = new Line2D(punti[imin],punti[imax]);
			return l.Point(t).y;
			}
		#endregion
		}
	}	// Fine namespace Fred68.Tools.Matematica
