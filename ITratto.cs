
namespace Fred68.Tools.Matematica
	{
	/// <summary> Interfaccia pe tutte le classi Tratto o derivate ad essa </summary>
	interface ITratto 
		{
		/// <summary>
		/// Proprieta` che restituisce il primo punto
		/// </summary>
		Point2D pStart {get;}
		/// <summary>
		/// Proprieta` che restituisce il secondo punto
		/// </summary>
		Point2D pEnd {get;}
		/// <summary>
		/// Proprieta` che restituisce un array con i due punti
		/// </summary>
		Point2D[] P12 {get;}
		bool Belongs(Point2D p, bool bInside);
		}
	}