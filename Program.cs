using Svg;
using System.Drawing;
using System.Drawing.Imaging;

namespace SVGtoImage
{
    internal class SVGtoImage
    {
        static void Main(string[] args)
        {
            string[] cmd = Environment.GetCommandLineArgs();
            string fileIn = "", Outputfile = "";
            ImageFormat imgFormat = ImageFormat.Png;
            int width = 0, height = 0, escala = 1;
            bool escalar = false, error = false, helper = false, exp = false;
            for (int i = 1; i < cmd.Length; i++)
            {
                string item = cmd[i];
                if (item == "-i")
                {
                    if (cmd.Length > i + 1)
                    {
                        if (new FileInfo(cmd[i + 1]).Exists)
                        {
                            fileIn = cmd[i + 1];
                        }
                        else
                        {
                            Console.WriteLine("ERROR:Argumento No Valido #1");
                            error = true;
                        }
                    }
                    else
                    {
                        Console.WriteLine("ERROR:Argumento fuera de rango #1");
                        error = false;
                    }
                }
                else if (item == "-W")//El Ancho de la imagen
                {
                    if (cmd.Length > i + 1)
                    {
                        if (IsDigit(cmd[i + 1]))
                        {
                            width = Convert.ToInt32(cmd[i + 1]);
                        }
                        else
                        {
                            Console.WriteLine("ERROR:Argumento No Valido #2");
                            error = true;
                        }
                    }
                    else
                    {
                        Console.WriteLine("ERROR:Argumento fuera de rango #2");
                        error = false;
                    }
                }
                else if (item == "-H")//El alto de la imagen
                {
                    if (cmd.Length > i + 1)
                    {
                        if (IsDigit(cmd[i + 1]))
                        {
                            height = Convert.ToInt32(cmd[i + 1]);
                        }
                        else
                        {
                            Console.WriteLine("ERROR:Argumento No Valido #3");
                            error = true;
                        }
                    }
                    else
                    {
                        Console.WriteLine("ERROR:Argumento fuera de rango #3");
                        error = false;
                    }
                }
                else if (item == "-O")//Seleciona la ruta de salida
                {
                    if (cmd.Length > i + 1)
                    {
                        if (new FileInfo(cmd[i + 1]).Exists)
                        {
                            Outputfile = cmd[i + 1];
                        }
                        else
                        {
                            Console.WriteLine("ERROR:Argumento No Valido #4");
                            error = true;
                        }
                    }
                    else
                    {
                        Console.WriteLine("ERROR:Argumento fuera de rango #4");
                        error = false;
                    }
                }
                else if (item == "-F")//Seleciona el formato de la imagen
                {
                    if (cmd.Length > i + 1)
                    {
                        if (FormatValid(cmd[i + 1]))
                        {
                            string format = cmd[i + 1];
                            imgFormat = format switch
                            {
                                "png" => ImageFormat.Png,
                                "jpeg" => ImageFormat.Jpeg,
                                "bmp" => ImageFormat.Bmp,
                                "tiff" => ImageFormat.Tiff,
                                _ => ImageFormat.Png,
                            };
                        }
                        else
                        {
                            Console.WriteLine("ERROR:Argumento No Valido #5");
                            error = true;
                        }
                    }
                    else
                    {
                        Console.WriteLine("ERROR:Argumento fuera de rango #5");
                        error = false;
                    }
                }
                else if (item == "-E")//Configura cuantas veces escalara el vector
                {
                    if (cmd.Length > i + 1)
                    {
                        if (IsDigit(cmd[i + 1]))
                        {
                            escalar = true;
                            escala = Convert.ToInt32(cmd[i + 1]);
                        }
                        else
                        {
                            Console.WriteLine("ERROR:Argumento No Valido #6");
                            error = true;
                        }
                    }
                    else
                    {
                        Console.WriteLine("ERROR:Argumento fuera de rango #6");
                        error = false;
                    }
                }
                else if (fileIn == "" && PathValid(item))
                {
                    fileIn = item;
                }
                else if (item == "-h")
                {
                    helper = true;
                }
                else if (item == "--exp")
                {
                    exp = true;
                }
            }

            if (exp)
            {//imprime los posibles errores
                Console.WriteLine(ExceptionHelp());
                return;
            }
            if (helper)//solo imprime la ayuda
            {
                Console.WriteLine(Help());
            }
            else
            {
                if (error == false)
                {
                    Console.WriteLine("Procesando:{0}\tPor favor espere...", fileIn);
                    if (Outputfile == "")//Si no se ha dado el argumento OutpuFile se configura automaticamente
                        Outputfile = fileIn.Remove(fileIn.Length-4) + "." + imgFormat.ToString().ToLower();
                    if (PathValid(fileIn))
                    {
                        if (new FileInfo(fileIn).Exists)//comprobar si el archivo existe
                        {
                            if (new FileInfo(fileIn).Extension == ".svg")//comprobar si la extension existe
                            {
                                FileInfo gf = new FileInfo(Outputfile);
                                if (gf.DirectoryName != null)
                                {
                                    if (new DirectoryInfo(gf.DirectoryName).Exists)//comprobar si el directorio existe
                                    {
                                        if (escalar)//si se reescalara la imagen(el tamaño de la imagen se multiplicara segun el valor asignado)
                                        {
                                            Image svgSave = SvgToImage(fileIn, escala);
                                            svgSave.Save(Outputfile, imgFormat);
                                            svgSave.Dispose();
                                        }
                                        else if (width > 0 && height > 0)//si se quiere un tamaño perzonalizado
                                        {
                                            Image svgSave = SvgToImage(fileIn, width, height);
                                            svgSave.Save(Outputfile, imgFormat);
                                            svgSave.Dispose();
                                        }
                                        else//tamaño predeterminado
                                        {
                                            Image svgSave = SvgToImage(fileIn);
                                            svgSave.Save(Outputfile, imgFormat);
                                            svgSave.Dispose();
                                        }
                                        Console.WriteLine("Se ha completado el proceso. Revisar:{0}", Outputfile);
                                    }
                                    else
                                    {
                                        Console.WriteLine("El directorio de salida no existe");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("El directorio de salida no es valido");
                                }
                            }
                            else
                            {
                                Console.WriteLine("El archivo de entrada no tiene un formato valido(El tipo de archivo debe ser SVG).");
                            }
                        }
                        else
                        {
                            Console.WriteLine("El archivo no existe.(Revisa la ruta completa)");
                        }
                    }
                    else
                    {
                        Console.WriteLine("La ruta es invalida");
                    }
                }
                else
                {
                    Console.WriteLine("ERROR");
                }
            }
        }

        public static Image SvgToImage(string pathFile)//Convierte un SVG en una Image
        {
            SvgDocument svgDocument = SvgDocument.Open(pathFile);
            Image bitmap = svgDocument.Draw();
            return bitmap;
        }
        public static Image SvgToImage(string pathFile, int escala)//Convierte un SVG en una Image
        {
            SvgDocument svgDocument = SvgDocument.Open(pathFile);
            SizeF sd = svgDocument.GetDimensions();
            int W = Convert.ToInt32(sd.Width) * escala;
            int H = Convert.ToInt32(sd.Height) * escala;
            Image bitmap = svgDocument.Draw(W, H);
            return bitmap;
        }
        public static Image SvgToImage(string pathFile, int rasterW, int rasterH)//Convierte un SVG en una Image
        {
            SvgDocument svgDocument = SvgDocument.Open(pathFile);
            Image bitmap = svgDocument.Draw(rasterW, rasterH);
            return bitmap;
        }
        public static bool IsDigit(string Text)//Comprueba si una cadena es un digito
        {
            bool Digit = true;
            for (int i = 0; i < Text.Length; i++)
            {
                if (!Char.IsDigit(Text[i]))
                {
                    Digit = false;
                    break;
                }
            }
            return Digit;
        }
        public static bool IsDigit(string Text, int start)//Comprueba si una cadena es un digito
        {
            bool Digit = true;
            for (int i = start; i < Text.Length; i++)
            {
                if (!Char.IsDigit(Text[i]))
                {
                    Digit = false;
                    break;
                }
            }
            return Digit;
        }
        public static bool IsDigit(string Text, int start, int end)//Comprueba si una cadena es un digito
        {
            bool Digit = true;
            for (int i = start; i < end; i++)
            {
                if (!Char.IsDigit(Text[i]))
                {
                    Digit = false;
                    break;
                }
            }
            return Digit;
        }
        public static bool FormatValid(string format)//devuele true, si el formato de la imagen esta registrado
        {
            bool valid = true;
            switch (format)
            {
                case "bmp":
                    valid = true;
                    break;
                case "jpeg":
                    valid = true;
                    break;
                case "png":
                    valid = true;
                    break;
                case "tiff":
                    valid = true;
                    break;
                default:
                    valid = false;
                    break;
            }
            return valid;
        }
        private static bool PathValid(string Path)//Comprueba si la ruta es valida obteniendo los primeros 4 caracteres
        {
            bool valid = false;
            if (Path.Length > 0)
            {
                if (Char.IsLetter(Path[0]))
                {
                    if (Path[1] == ':')
                    {
                        if (Path[2] == '\\')
                        {
                            if (Char.IsLetter(Path[3]))
                            {
                                FileInfo fi = new(Path);
                                if (fi.Exists)
                                {
                                    valid = true;
                                }else{
                                    valid = false;
                                }
                            }
                            else
                            {
                                valid = false;
                            }
                        }
                        else
                        {
                            valid = false;
                        }
                    }
                    else
                    {
                        valid = false;
                    }
                }
                else
                {
                    valid = false;
                }
            }
            else
            {
                valid = false;
            }
            return valid;
        }
        private static string Help()//Devueve la Ayuda general
        {
            string Start = "\t\t\tSVGNET 1.0.0\nDescripcion:Una herramienta que te permite convertir un svg en una imagen";
            string uso = "EJEMPLO SIMPLE:\tSVGTOIMAGE \"PathFile\"";
            string input = "-i\tArchivo SVG de entrada\nEjemplo: \"SVGNET -i C:\\users\\Username\\Document\\svgejemplo.svg\"";
            string Width = "-W\tEl ancho del archivo\nEjemplo: \"SVGNET -i \"pathfile\" -W 720 -H 720";
            string Height = "-H\tEl alto del archivo\nEjemplo: \"SVGNET -i \"pathfile\" -H 1080 -W 1080";
            string Format = "-F\tFormato de salida\nFormato Disponible: 1 - png, 2 - jpeg, 3 - bmp, 4 - tiff\nEjemplo: SVGNET -i \"pathfile\" -F jpeg";
            string Outputfile = "-O\tRuta de salida:\nEjemplo: \"SVGNET -i \"pathfile\" -O \"pathfileOutput\"\"\nNOTA:Por defecto, se guarda en la misma carpeta del archivo, con su mismo nombre.";
            string escalar = "-E\tEscala el tamaño del vector:\nEjemplo: \"SVGNET -i \"pathfile\" -E 4\"";
            string exp = "--exp\tMuestra informacion de algunos posibles errores.";
            string help = "-h\tMuestra la ayuda del software";
            String show = String.Format("{0}\n\n{1}\n\n{2}\n\n{3}\n\n{4}\n\n{5}\n\n{6}\n\n{7}\n\n{8}\n\n{9}", Start, uso, input, Outputfile, Format, escalar, Width, Height, exp, help);
            return show;
        }
        private static string ExceptionHelp()//Devuelve una serie de errores registrados
        {
            string Title = "\t\tLISTA DE POSIBLES ERRORES.";
            string error1 = "Argumento no valido #1: La ruta del archivo de entrada, no es valido.\n" + "Argumento fuera de rango #1: no se ha dado un argumento al parametro \"-i\"";
            string error2 = "Argumento no valido #2:El valor del ancho de la imagen es invalido.\n" + "Argumento fuera de rango #2: No se ha dado un valor del ancho \"-W\"";
            string error3 = "Argumento no valido #3:El valor de la altura de la imagen es invalido.\n" + "Argumento fuera de rango #3: No se ha dado un valor de la altura \"-H\"";
            string error4 = "Argumento no valido #4:La ruta de salida no es valida.\n" + "Argumento fuera de rango #4: No se ha dado ninguna ruta de salida donde se guardara el archivo \"-O\"";
            string error5 = "Argumento no valido #5:El Formato selecionado no es valido(formatos validos: bmp,jpeg,png,tiff).\n" + "Argumento fuera de rango #5: No se ha dado un valor de formato \"-F\"";
            string error6 = "Argumento no valido #6:El valor de la escala no se reconoce como un numero entero.\n" + "Argumento fuera de rango #6: No se ha dado ningun valor para escalar el vector \"-E\"";
            string ExceptionH = String.Format("{0}\n\n{1}\n\n{2}\n\n{3}\n\n{4}\n\n{5}\n\n{6}", Title, error1, error2, error3, error4, error5, error6);
            return ExceptionH;
        }
    }
}