using System;
using MySql.Data.MySqlClient;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Google.Protobuf.WellKnownTypes;



namespace Consola
{
    class Programa
    {
        static string connectionString = "server=localhost; database=biblioteca; user=root";

                static void Main(string[] args)
        {
            menu_principal();
        }
                static void mostrar_id()
{
    Console.WriteLine("¿Qué tipo de ID quiere obtener? ");
    Console.WriteLine("1) ID de usuario por DNI");
    Console.WriteLine("2) Buscar ID de libro por nombre");
    Console.WriteLine("3) Mostrar ID de géneros disponibles");
    Console.WriteLine("4) Mostrar ID de préstamo por ID de libro");
    Console.WriteLine("5) Volver al menú principal");
    int opcion_busqueda = Convert.ToInt32(Console.ReadLine());

    if (opcion_busqueda == 1)
    {
        Console.Write("Ingrese el DNI del usuario: ");
        string? dni = Console.ReadLine();

        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                string query = "SELECT usuario_id FROM usuarios WHERE dni = @dni";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@dni", dni);
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        Console.WriteLine($"El ID del usuario {dni} es: {result}");
                    }
                    else
                    {
                        Console.WriteLine("Error, no se encontró un usuario con ese DNI.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al conectar con la base de datos: {ex.Message}");
            }
        }
    }
    else if (opcion_busqueda == 2)
    {
        Console.Write("Ingrese el nombre del libro: ");
        string? titulo_libro = Console.ReadLine();

        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                string query = "SELECT libro_id FROM libros WHERE nombre = @nombre";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", titulo_libro);
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        Console.WriteLine($"El ID del libro '{titulo_libro}' es: {result}");
                    }
                    else
                    {
                        Console.WriteLine("No se encontró un libro con ese nombre.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al conectar con la base de datos: {ex.Message}");
            }
        }
    }
    else if (opcion_busqueda == 3)
    {
        Console.WriteLine("Mostrando géneros disponibles...");

        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                string query = "SELECT genero_id, descripcion FROM generos";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("ID - Descripción del Género:");
                            while (reader.Read())
                            {
                                int genero_id = reader.GetInt32("genero_id");
                                string descripcion = reader.GetString("descripcion");
                                Console.WriteLine($"{genero_id} - {descripcion}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No se encontraron géneros disponibles.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al conectar con la base de datos: {ex.Message}");
            }
        }
    }
    else if (opcion_busqueda == 4) 
    {
        Console.Write("Ingrese el ID del libro para obtener el ID del préstamo: ");
        int libroId = Convert.ToInt32(Console.ReadLine()); 

        string query = "SELECT prestamo_id FROM prestamos WHERE libro_id = @libro_id";

        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@libro_id", libroId);

            try
            {
                conn.Open();

                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    Console.WriteLine($"El ID del préstamo para el libro con ID {libroId} es: {result}");
                }
                else
                {
                    Console.WriteLine("No se encontró un préstamo para el libro con ese ID.");
                }
            }
            catch (MySqlException sqlEx)
            {
                Console.WriteLine("Error de SQL: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocurrió un error: " + ex.Message);
            }
        }
    }
    else if (opcion_busqueda == 5)
    {
        Console.WriteLine("Volviendo al menú principal...");
        return;
    }
    else
    {
        Console.WriteLine("Por favor introduzca un número válido entre 1 y 5.");
    }
}
                static void crear_usuario()
            {string? nombre, apellido, dni, telefono, email;
                Console.WriteLine("Ingrese nombre: ");
                nombre = Console.ReadLine();
                Console.WriteLine("Ingrese apellido: ");
                apellido = Console.ReadLine();
                Console.WriteLine("Ingrese dni: ");
                dni = Console.ReadLine();
                Console.WriteLine("Ingrese telefono: ");
                telefono = Console.ReadLine();
                Console.WriteLine("Ingrese email: ");
                email = Console.ReadLine();

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        Console.WriteLine("Conexion exitosa");
                        string query = "INSERT INTO usuarios (nombre,apellido,dni,telefono,email) " + "VALUES (@nombre, @apellido,@dni,@telefono,@email)";


                        using (MySqlCommand command = new MySqlCommand(query, conn))
                        {
                            command.Parameters.AddWithValue("@nombre", nombre);
                            command.Parameters.AddWithValue("@apellido", apellido);
                            command.Parameters.AddWithValue("@dni", dni);
                            command.Parameters.AddWithValue("@telefono", telefono);
                            command.Parameters.AddWithValue("@email", email);


                            int resultado = command.ExecuteNonQuery();

                            if (resultado > 0)
                            {
                                long usuario_id = command.LastInsertedId;
                                Console.WriteLine("\t\t\tRegistro agregado correctamente!....");
                                Console.WriteLine($"\t\t\tSu usuario de id es: {usuario_id}");
                            }
                            else
                            {
                                Console.WriteLine("\t\t\tError al iniciar el registro!...");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
                static void actualizar_usuario()
            { 
                int opciones,idUsuario;
                string? nombre="", apellido="",dni1="", telefono="",email="";

                Console.WriteLine("Ingrese el id del usuario:");
               idUsuario= Convert.ToInt32(Console.ReadLine());

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        Console.WriteLine("Conexion exitosa");
                        opciones = -1;
                            while (opciones != 0)
                            {
                                Console.Clear();
                                Console.WriteLine("\\Elija una opcion para el dato que quiera actualizar: ");
                                Console.WriteLine("1) Actualizar el nombre del usuario");
                                Console.WriteLine("2) Actualizar el apellido del usuario ");
                                Console.WriteLine("3) Actualizar el dni del usuario ");
                                Console.WriteLine("4) Actualizar el telefono del usuario ");
                                Console.WriteLine("5) Actualizar el email del usuario ");
                                Console.WriteLine("6) Salir .. \" ");
                                opciones = Convert.ToInt32(Console.ReadLine());
                                Console.Clear();

                                switch (opciones)
                                {
                                    case 1:
                                    Console.WriteLine("Ingrese el nuevo nombre");
                                    nombre = Console.ReadLine();
                                    string query1 = "UPDATE usuarios SET nombre = @nuevoNombre WHERE usuario_id = @idUsuario";
                                        using (MySqlCommand command = new MySqlCommand(query1,conn))
                                        {
                                            command.Parameters.AddWithValue("@nuevoNombre", nombre);
                                        command.Parameters.AddWithValue("@idUsuario", idUsuario);
                                        int estado = command.ExecuteNonQuery();
                                        if (estado > 0)
                                        {
                                            Console.WriteLine("\t\t\tUsuario actualizado correctamente!....");
                                        }
                                        else
                                        {
                                            Console.WriteLine("\t\t\tError al actualizar el usuario !...");
                                        }
                                    } break;
                                    case 2:
                                    Console.WriteLine("Ingrese su nuevo apellido: ");
                                    apellido = Console.ReadLine();
                                    string query2 = "UPDATE usuarios SET apellido = @nuevoApellido usuario_id = @idUsuario";

                                    using (MySqlCommand command = new MySqlCommand(query2, conn))
                                    {
                                        command.Parameters.AddWithValue("@nuevoApellido", apellido);
                                        command.Parameters.AddWithValue("@idUsuario", idUsuario);
                                        int estado = command.ExecuteNonQuery();
                                        if (estado > 0) { Console.WriteLine("Usuario Actializado correctamente...."); }
                                        else { Console.WriteLine("Error al actualizar el usuario... "); } break;
                                    }
                                case 3:
                                            Console.WriteLine("Ingrese su nuevo Dni: ");
                                            dni1=Console.ReadLine();
                                            string query3 = "UPDATE usuarios SET dni1 = @nuevoDni WHERE usuario_id = @idUsuario";

                                            using (MySqlCommand command = new MySqlCommand(query3, conn))
                                            { command.Parameters.AddWithValue("@nuevoDni", dni1);
                                                command.Parameters.AddWithValue("@idUsuario", idUsuario);
                                                int estado = command.ExecuteNonQuery();
                                                if (estado > 0) { Console.WriteLine("Usuario dni actualizado correctamente...");}
                                                else{Console.WriteLine("Error al actualizar el usuario");} break;
                                            }
                                case 4:
                                    Console.WriteLine("Ingrese el nuevo telefono:");
                                    telefono = Console.ReadLine();
                                    string query4 = "UPDATE usuarios SET telefono = @nuevoTelefono WHERE usuario_id = @idUsuario";
                                    using (MySqlCommand command = new MySqlCommand(query4, conn))
                                    {
                                        command.Parameters.AddWithValue("@nuevoTelefono", telefono);
                                        command.Parameters.AddWithValue("@idUsuario", idUsuario);
                                        int estado = command.ExecuteNonQuery();
                                        if (estado > 0)
                                        {
                                            Console.WriteLine("\t\t\tUsuario actualizado correctamente!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("\t\t\tError al actualizar el usuario!");
                                        }
                                    }
                                    break;
                                case 5:
                                    Console.WriteLine("Ingrese el nuevo email:");
                                    email = Console.ReadLine();
                                    string query5 = "UPDATE usuarios SET email = @nuevoEmail WHERE usuario_id = @idUsuario";
                                    using (MySqlCommand command = new MySqlCommand(query5, conn))
                                    {
                                        command.Parameters.AddWithValue("@nuevoEmail", email);
                                        command.Parameters.AddWithValue("@idUsuario", idUsuario);
                                        int estado = command.ExecuteNonQuery();
                                        if (estado > 0)
                                        {
                                            Console.WriteLine("\t\t\tUsuario actualizado correctamente!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("\t\t\tError al actualizar el usuario!");
                                        }
                                    } break;
                                case 6:
                                    opciones = 0;
                                    break;

                                default:
                                    Console.WriteLine("Opción no válida.");
                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
                static void borrar_usuario()
                {
                    string? idusuario;
                    Console.WriteLine("Ingrese el id del usuario: ");
                    idusuario = Console.ReadLine();


                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        try
                        {
                            conn.Open();
                            Console.WriteLine("Conexion exitosa");
                            string query = "UPDATE usuarios SET estado = @nuevoestado WHERE usuario_id = @idusuario AND estado = 1";
                            

                            using (MySqlCommand command = new MySqlCommand(query, conn))
                            {
                                command.Parameters.AddWithValue("@nuevoestado", 0);
                                command.Parameters.AddWithValue("@idusuario", idusuario);


                                int estado1 = command.ExecuteNonQuery();

                                if (estado1 > 0)
                                {
                                    Console.WriteLine("\t\t\tUsuario borrado correctamente!....");
                                }
                                else
                                {
                                    Console.WriteLine("\t\t\tError al borrar el usuario !...");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                }
                static void agregar_libro()
        {
            string? nombre_libro, autor_libro, fecha_lanzamiento;
            int genero_libro;

            Console.WriteLine("\nIngrese el nombre del nuevo libro: \n");
            nombre_libro = Console.ReadLine();

            Console.WriteLine("Ingrese el autor del nuevo libro: \n");
            autor_libro = Console.ReadLine();

            Console.WriteLine("Ingrese el ID del género del nuevo libro: ");
            genero_libro = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Ingrese la fecha de lanzamiento del libro (el formato es: YYYY-MM-DD) Agregue guiones medios entre campos (obligatoriamente): ");
            fecha_lanzamiento = Console.ReadLine();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    Console.WriteLine("Conexión exitosa");

                    string query = "INSERT INTO libros (nombre, autor, generos, fecha_lanzamiento, estado) " +
                                   "VALUES (@nombre, @autor, @generoid, @fecha_lanzamiento, @estado)";

                    using (MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@nombre", nombre_libro);
                        command.Parameters.AddWithValue("@autor", autor_libro);
                        command.Parameters.AddWithValue("@generoid", genero_libro); 
                        command.Parameters.AddWithValue("@fecha_lanzamiento", fecha_lanzamiento);
                        command.Parameters.AddWithValue("@estado", 1);

                        int resultado = command.ExecuteNonQuery();

                        if (resultado > 0)
                        {
                            long libro_id = command.LastInsertedId;
                            Console.WriteLine("\t\t\tRegistro de libro agregado correctamente!");
                            Console.WriteLine($"\t\t\tEl ID del libro es: {libro_id}");
                        }
                        else
                        {
                            Console.WriteLine("\t\t\tError al iniciar el registro del libro...");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ocurrió un error: {ex.Message}");
                }
            }
        }
                static void actualizar_libro()
        {
            string? nombre_libro = "";
            string? autor_libro = "";
            int? genero;
            int libro_id = 0;
            int opciones = 0;

            Console.WriteLine("Ingrese el id del libro a buscar:");
            libro_id = Convert.ToInt32(Console.ReadLine());

                using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    Console.WriteLine("Conexión exitosa");

                    opciones = -1;
                    while (opciones != 4)
                    {
                        Console.Clear();
                        Console.WriteLine("Elija una opción para el dato que quiera actualizar: ");
                        Console.WriteLine("1) Actualizar el nombre del libro");
                        Console.WriteLine("2) Actualizar el apellido del autor del libro");
                        Console.WriteLine("3) Actualizar el género del libro");
                        Console.WriteLine("4) Salir..");
                        opciones = Convert.ToInt32(Console.ReadLine());
                        Console.Clear();

                        switch (opciones)
                        {
                            case 1:
                                Console.WriteLine("Ingrese el nuevo nombre:");
                                nombre_libro = Console.ReadLine();
                                string query1 = "UPDATE libros SET nombre = @nuevoNombre WHERE libro_id = @libroId";
                                using (MySqlCommand command = new MySqlCommand(query1, conn))
                                {
                                    command.Parameters.AddWithValue("@nuevoNombre", nombre_libro);
                                    command.Parameters.AddWithValue("@libroId", libro_id);
                                    int estado = command.ExecuteNonQuery();
                                    if (estado > 0)
                                    {
                                        Console.WriteLine($"\t\t\tNombre de libro actualizado correctamente!.... {nombre_libro}");
                                    }
                                    else
                                    {
                                        Console.WriteLine("\t\t\tError al actualizar el libro!...");
                                    }
                                }
                                break;

                            case 2:
                                Console.WriteLine("Ingrese el nuevo apellido del autor: ");
                                autor_libro = Console.ReadLine();
                                string query2 = "UPDATE libros SET autor = @nuevoApellido WHERE libro_id = @libroId";
                                using (MySqlCommand command = new MySqlCommand(query2, conn))
                                {
                                    command.Parameters.AddWithValue("@nuevoApellido", autor_libro);
                                    command.Parameters.AddWithValue("@libroId", libro_id);
                                    int estado = command.ExecuteNonQuery();
                                    if (estado > 0)
                                    {
                                        Console.WriteLine("Apellido del autor actualizado correctamente....");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Error al actualizar el libro...");
                                    }
                                }
                                break;

                            case 3:
                                Console.WriteLine("Ingrese el nuevo género del libro:");
                                genero = Convert.ToInt32(Console.ReadLine());

                                string query3 = "UPDATE libros SET genero = @nuevoGenero WHERE libro_id = @libroId";
                                using (MySqlCommand command = new MySqlCommand(query3, conn))
                                {
                                    command.Parameters.AddWithValue("@nuevoGenero", genero);
                                    command.Parameters.AddWithValue("@libroId", libro_id);
                                    int estado = command.ExecuteNonQuery();
                                    if (estado > 0)
                                    {
                                        Console.WriteLine("Género del libro actualizado correctamente...");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Error al actualizar el libro");
                                    }
                                }
                                break;

                            case 4:
                                Console.WriteLine("Hasta luego, vuelva pronto.");
                                return;

                            default:
                                Console.WriteLine("Opción no válida. Por favor, seleccione una opción entre 1 y 4.");
                                break;
                            }
                        }
                        }
                        catch (Exception ex)
                        {
                        Console.WriteLine(ex.ToString());
                        }
                        }
                    }       
                static void borrar_libro()
                {
                    string? libro_id;
                    Console.WriteLine("Ingrese el id del libro: ");
                    libro_id = Console.ReadLine();


                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        try
                        {
                            conn.Open();
                            Console.WriteLine("Conexion exitosa");
                            string query = "UPDATE libros SET estado = @nuevoestado WHERE usuario_id = @idusuario AND estado = 1";


                            using (MySqlCommand command = new MySqlCommand(query, conn))
                            {
                                command.Parameters.AddWithValue("@nuevoestado", 0);
                                command.Parameters.AddWithValue("@idusuario", libro_id);


                                int estado1 = command.ExecuteNonQuery();

                                if (estado1 > 0)
                                {
                                    Console.WriteLine("\t\t\tLibro borrado correctamente!....");
                                }
                                else
                                {
                                    Console.WriteLine("\t\t\tError al borrar el usuario !...");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                }
                static void agregar_genero()
                {
                 Console.Write("Ingrese el género a agregar: ");
                string? genero = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(genero))
                {
                Console.WriteLine("El género no puede estar vacío.");
                return;
                }

                string query = "INSERT INTO generos (descripcion) VALUES (@genero)";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@genero", genero);

                    try
                    {
                        connection.Open();
                        int filasAfectadas = cmd.ExecuteNonQuery();
                        Console.WriteLine(filasAfectadas > 0 ? "\nGénero agregado con éxito." : "\nNo se pudo agregar el nuevo género.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Ocurrió un error: " + ex.Message);
                    }
                }
                }
                }
                static void actualizar_genero()
        {
            string? nueva_descripcion = "";
            int genero_id = 0;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    Console.WriteLine("Conexión exitosa");

                    // Mostrar los géneros disponibles
                    Console.WriteLine("Generos disponibles:");
                    string queryMostrar = "SELECT genero_id, descripcion FROM generos";
                    using (MySqlCommand command = new MySqlCommand(queryMostrar, conn))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine($"ID: {reader["genero_id"]}, Descripción: {reader["descripcion"]}");
                            }
                        }
                    }

                    Console.WriteLine("\nIngrese el ID del género que desea actualizar:");
                    if (!int.TryParse(Console.ReadLine(), out genero_id))
                    {
                        Console.WriteLine("ID inválido. Operación cancelada.");
                        return;
                    }

                    // Solicitar la nueva descripción
                    Console.WriteLine("Ingrese la nueva descripción para el género:");
                    nueva_descripcion = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(nueva_descripcion))
                    {
                        Console.WriteLine("La descripción no puede estar vacía.");
                        return;
                    }

                    // Actualizar el género
                    string queryActualizar = "UPDATE generos SET descripcion = @nuevaDescripcion WHERE genero_id = @generoId";

                    using (MySqlCommand command = new MySqlCommand(queryActualizar, conn))
                    {
                        command.Parameters.AddWithValue("@nuevaDescripcion", nueva_descripcion);
                        command.Parameters.AddWithValue("@generoId", genero_id);

                        int filasAfectadas = command.ExecuteNonQuery();
                        if (filasAfectadas > 0)
                        {
                            Console.WriteLine("Descripción del género actualizada correctamente.");
                        }
                        else
                        {
                            Console.WriteLine("No se encontró un género con el ID especificado.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ocurrió un error: {ex.Message}");
                }
            }
        }
                static void crear_prestamo()
        {
            int usuarioid;
            int libroid;

            Console.Write("Ingrese el ID del usuario: ");
            usuarioid = Convert.ToInt32(Console.ReadLine());

            Console.Write("Ingrese el ID del libro: ");
            libroid = Convert.ToInt32(Console.ReadLine());

            string queryVerificar = @"SELECT usuarios.estado AS usuarioestado, libros.estado AS LibroEstado 
                              FROM usuarios CROSS JOIN libros 
                              WHERE usuario_id = @usuarioid AND libro_id = @libroid";

            string queryInsertar = "INSERT INTO prestamos (fecha_prestamo, fecha_estimada, libro_id, usuario_id) VALUES " +
                                   "(@fechaestimada, NULL, @libroid, @usuarioid)";

            
            string queryActualizarLibro = "UPDATE libros SET estado = 0 WHERE libro_id = @libroid";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(queryVerificar, connection);
                cmd.Parameters.AddWithValue("@usuarioid", usuarioid);
                cmd.Parameters.AddWithValue("@libroid", libroid);

                try
                {
                    connection.Open();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            sbyte usuarioEstado = reader["usuarioestado"] != DBNull.Value ? Convert.ToSByte(reader["usuarioestado"]) : (sbyte)-1;
                            sbyte libroEstado = reader["LibroEstado"] != DBNull.Value ? Convert.ToSByte(reader["LibroEstado"]) : (sbyte)-1;

                            if (usuarioEstado == 1 && libroEstado == 1)
                            {
                                reader.Close();

                                
                                using (var transaction = connection.BeginTransaction())
                                {
                                    try
                                    {
                                        
                                        MySqlCommand cmdInsertar = new MySqlCommand(queryInsertar, connection, transaction);
                                        cmdInsertar.Parameters.AddWithValue("@usuarioid", usuarioid);
                                        cmdInsertar.Parameters.AddWithValue("@libroid", libroid);
                                        cmdInsertar.Parameters.AddWithValue("@fechaestimada", DateTime.Now.AddDays(7));
                                        int filaAfectadaPrestamo = cmdInsertar.ExecuteNonQuery();

                                        MySqlCommand Actualizar = new MySqlCommand(queryActualizarLibro, connection, transaction);
                                        Actualizar.Parameters.AddWithValue("@libroid", libroid);
                                        int filaAfectadaLibro = Actualizar.ExecuteNonQuery();

                                        if (filaAfectadaPrestamo > 0 && filaAfectadaLibro > 0)
                                        {
                                            transaction.Commit();
                                            Console.WriteLine("\nPréstamo creado con éxito y estado del libro actualizado.");
                                        }
                                        else
                                        {
                                            transaction.Rollback();
                                            Console.WriteLine("\nNo se pudo crear el préstamo o actualizar el estado del libro.");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        transaction.Rollback();
                                        Console.WriteLine("Error al realizar la transacción: " + ex.Message);
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("\nEl préstamo no se puede realizar porque el usuario o el libro no están activos.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No se encontraron registros para el usuario o el libro proporcionados");
                        }
                    }
                }
                catch (MySqlException sqlEx)
                {
                    Console.WriteLine("Error de SQL: " + sqlEx.Message);
                    Console.WriteLine("Detalles: " + sqlEx.StackTrace);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocurrió un error: " + ex.Message);
                    Console.WriteLine("Detalles: " + ex.StackTrace);
                }
            }
        }
                static void actualizar_prestamo()
        {
            int? prestamo_id;
            DateTime nuevaFechaEstimada;

            Console.Write("Ingrese el ID del préstamo que desea actualizar: ");
            prestamo_id = Convert.ToInt32(Console.ReadLine());

            Console.Write("Ingrese la nueva fecha estimada (formato obligatorio: YYYY-MM-DD): ");
            string? fechaInput = Console.ReadLine();

            
            if (DateTime.TryParse(fechaInput, out nuevaFechaEstimada))
            {
                string queryActualizar = "UPDATE prestamos SET fecha_estimada = @nuevaFechaEstimada WHERE prestamo_id = @prestamoid";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand(queryActualizar, connection);
                    cmd.Parameters.AddWithValue("@prestamoid", prestamo_id);
                    cmd.Parameters.AddWithValue("@nuevaFechaEstimada", nuevaFechaEstimada);

                    try
                    {
                        connection.Open();

                        int filasAfectadas = cmd.ExecuteNonQuery();

                        if (filasAfectadas > 0)
                        {
                            Console.WriteLine($"\nEl préstamo ha sido actualizado exitosamente." +
                                $"La fecha estimada de devolucion es: {nuevaFechaEstimada}");
                        }
                        else
                        {
                            Console.WriteLine("\nNo se encontró un préstamo con ese ID, o no se realizaron cambios.");
                        }
                    }
                    catch (MySqlException sqlEx)
                    {
                        Console.WriteLine("Error de SQL: " + sqlEx.Message);
                        Console.WriteLine("Detalles: " + sqlEx.StackTrace);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Ocurrió un error: " + ex.Message);
                        Console.WriteLine("Detalles: " + ex.StackTrace);
                    }
                }
            }
            else
            {
                Console.WriteLine("La fecha ingresada no tiene un formato válido. Por favor ingrese una fecha válida.");
            }
        }
                static void menu_principal()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                int opcion_menu = -1;

                while (opcion_menu != 12)
                {
                    Console.Clear();
                    Console.WriteLine("Bienvenido al menú principal:");
                    Console.WriteLine("1) Mostrar ID de usuario, id de libro o id del genero");
                    Console.WriteLine("2) Crear un usuario nuevo");
                    Console.WriteLine("3) Actualizar un usuario");
                    Console.WriteLine("4) Borrar usuario");
                    Console.WriteLine("5) Agregar libro");
                    Console.WriteLine("6) Actualizar libro");
                    Console.WriteLine("7) Borrar libro");
                    Console.WriteLine("8) Agregar género");
                    Console.WriteLine("9) Actualizar género");
                    Console.WriteLine("10) Crear préstamo");
                    Console.WriteLine("11) Actualizar préstamo");
                    Console.WriteLine("12) Salir");

                    
                    if (!int.TryParse(Console.ReadLine(), out opcion_menu))
                    {
                        Console.WriteLine("Por favor, ingrese un número válido.");
                        Console.WriteLine("Presione Enter para continuar...");
                        Console.ReadLine();
                        continue;
                    }

                    Console.Clear();
                    switch (opcion_menu)
                    {
                        case 2: crear_usuario();
                            break;
                        case 3: actualizar_usuario();
                            break;
                        case 4: borrar_usuario();
                            break;
                        case 5: agregar_libro();
                            break;
                        case 6: actualizar_libro();
                            break;
                        case 7: borrar_libro();
                            break;
                        case 8: agregar_genero();
                            break;
                        case 9: actualizar_genero();
                            break;
                        case 10: crear_prestamo();
                            break;
                        case 11: actualizar_prestamo();
                            break;
                        case 1: mostrar_id();
                            break;
                        case 12: Console.WriteLine("Gracias, vuelva pronto...");
                            break; 
                        default:
                            Console.WriteLine("Opción no válida. Por favor, seleccione un número entre 1 y 12.");
                            break;
                    }

                    if (opcion_menu != 12)
                    {
                        Console.WriteLine("Presione Enter para continuar...");
                        Console.ReadLine();
                    }
                }
            }
        }
    }
}


