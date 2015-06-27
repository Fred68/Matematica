using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fred68.Tools.Matematica
	{

	/// <summary>
	/// Delegate della funzione N->1 MxN->1 o 1->1
	/// di cui trovare le soluzioni (minimo, massimo, zeri ecc...)
	/// </summary>
	/// <param name="m">Input data</param>
	/// <returns></returns>
	delegate double Funzione(Matrix m);

	/// <summary>
	/// Classe con i dati comuni per i risolutori (precisione, passi ecc...)
	/// </summary>
	class SolverData
		{
		/// <summary>
		/// Criterio di arresto |f| &lt; eps
		/// </summary>
		double eps_funzione;
		/// <summary>
		/// Criterio di arresto: |a[i]-b[i]| &lt; eps
		/// </summary>
		double eps_intervallo;
		/// <summary>
		/// Criterio di arresto: |x[n] - x[n-1]| &lt; eps
		/// </summary>
		double eps_incremento;
		/// <summary>
		/// Criterio di arresto: IT &lt; ITmax
		/// </summary>
		int max_iterazioni;
		/// <summary>
		/// Criterio di arresto relativo: |a[i]-b[i]| &lt; eps * |a[i]+b[i]|
		/// </summary>
		bool intervallo_relativo;
		/// <summary>
		/// Criterio di arresto relativo: |x[n] - x[n-1]| &lt; eps * |x[n] + x[n-1]|
		/// </summary>
		bool incremento_relativo;
		/// <summary>
		/// Usa media mobile
		/// </summary>
		bool media_mobile;
		/// <summary>
		/// Numero di campioni per criterio su media mobile
		/// </summary>
		int n_media_mobile;

		public const double EPS_FUNZIONE = 1E-10;
		public const double EPS_INTERVALLO = 1E-10;
		public const double EPS_INCREMENTO = 1E-10;
		public const int MAX_ITERAZIONI = 10000;
		public const int MEDIA_MOBILE = 4;

		public SolverData()
			{
			eps_funzione = EPS_FUNZIONE;
			eps_intervallo = EPS_INTERVALLO;
			eps_incremento = EPS_INCREMENTO;
			max_iterazioni = MAX_ITERAZIONI;
			intervallo_relativo = false;
			incremento_relativo = false;
			media_mobile = false;
			n_media_mobile = MEDIA_MOBILE;
			}
		}


	/// <summary>
	/// Ricerca zero 
	/// </summary>
	class Bisezione : SolverData
		{
		
		}
	}
