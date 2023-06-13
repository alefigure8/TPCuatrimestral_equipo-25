﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
	public class Usuario
	{
		public int Id { get; set; }
		public string Nombres { get; set; }
		public string Apellidos { get; set; }
		public string Mail { get; set; }
		public string Password { get; set; }
		public TipoUsuarios Tipo { get; set; }
	}
}