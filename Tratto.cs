using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Fred68.Tools.Matematica;
using Fred68.Tools.Utilita;
using Fred68.Tools.Grafica;

namespace Fred68.Tools.Matematica
	{
	/// <summary> Classe base per segmenti di linea (Line2D, Arc2D ecc....)
	/// Eredita interfaccia ITratto, ma la soddisfa con proprieta` astratte (da implementare nella classe derivata)
	/// La classe e` astratta (non puo` essere istanziata)
	/// </summary>
	public abstract class Tratto : ITratto, IValid, IPlot
		{
		/// <summary>
		/// Tipo del dato contenuto nell'oggetto derivato
		/// </summary>
		protected Type tipo;
		/// <summary>
		/// Costruttore (inizializza a null il tipo)
		/// </summary>
		public Tratto() 
			{
			tipo = null;
			}
		/// <summary>
		/// Costruttore protetto, chiamato dal costruttore della classe derivata
		/// </summary>
		/// <param name="t">Tipo di dato (passato come parametro dal costruttore della classe derivata)</param>
		protected Tratto(Type t) 
			{
			tipo = t;
			}
		/// <summary>
		/// Proprieta` astratta
		/// </summary>
		public abstract Point2D pStart 
			{
			get;
			}
		/// <summary>
		/// Proprieta` astratta
		/// </summary>
		public abstract Point2D pEnd 
		    {
		    get;
			}
		/// <summary>
		/// Funzione astratta
		/// </summary>
		public abstract void Validate();
		/// <summary>
		/// Proprieta` astratta
		/// </summary>
		public abstract bool IsValid 
			{
			get;
			}
		/// <summary>
		/// Proprieta` astratta
		/// </summary>
		public abstract Point2D[] P12 
			{
			get;
			}
		/// <summary>
		/// Funzione astratta
		/// </summary>
		public abstract bool Belongs(Point2D p, bool bInside);
		/// <summary>
		/// Funzione astratta
		/// </summary>
		public abstract void Plot(Graphics dc, Finestra fin, Pen penna);
		/// <summary>
		/// Funzione astratta
		/// </summary>
		public abstract void Display(DisplayList displaylist, int penna);
		}
	}