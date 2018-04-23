using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTree
{
    public delegate int CompareTo<T>(T value, T value2);
    public class BTNode<T>
    {
        public BTNode()
        {
            Values = new T[5];
            Children = new BTNode<T>[5];
            Cuenta = 0;
        }

        public T[] Values { get; set; }
        public BTNode<T>[] Children { get; set; }
        public int Cuenta { get; set; }
    }
    public class Btree<T>
    {
        #region variables
        public int Min { get; set; }
        public int Max { get; set; }
        public int Orden { get; set; }
        public BTNode<T> root { get; set; }
        public Lazy<List<T>> list;
        public CompareTo<T> comparador;
        #endregion

        #region Constructores
        public Btree(int Orden, CompareTo<T> compare)
        {
            this.Orden = Orden;
            Min = (Orden - 1) / 2;
            Max = Orden - 1;
            this.root = null;
            this.list = new Lazy<List<T>>();
            this.comparador = compare;
        }
        public Btree()
        {
            root = null;
        }
        #endregion

        #region Búsqueda
        public static bool ArbolVacio(BTNode<T> node) => node == null;
        private void BuscarNodo(T clave, BTNode<T> P, ref bool Encontrado, ref int k)
        {
            if (comparador(clave, P.Values[1]) == -1)
            {
                Encontrado = false;
                k = 0; // rama por donde continuar
            }
            else
            {
                //revisa valores del nodo
                k = P.Cuenta;

                while (comparador(clave, P.Values[k]) == -1 && k > 1)
                {

                    k = k - 1;
                    Encontrado = (comparador(clave, P.Values[k]) == 0);
                }
            }
        }
        public void Buscar(T clave, BTNode<T> root, ref bool encontrado, ref BTNode<T> N, ref int posición)
        {

            if (ArbolVacio(root))
            {
                encontrado = false;
            }
            else
            {
                BuscarNodo(clave, root, ref encontrado, ref posición);
                if (encontrado)
                {
                    N = root;
                }
                else
                {
                    Buscar(clave, root.Children[posición], ref encontrado, ref N, ref posición);
                }
            }
        }

        #endregion

        #region Inserción
        /// <summary>
        /// Este proceso se inicia cuando ya se ha encontrado espacio
        /// en el nodo 
        /// </summary>
        /// <param name="x"></param> Clave a insertar
        /// <param name="Xder"></param> Dirección del nodo
        /// <param name="P"></param> Nodo sucesor
        /// <param name="K"></param> Posición donde se insertará
        private void MeterHoja(T x, BTNode<T> Xder, BTNode<T> P, int K)
        {
            for (int i = P.Cuenta; i >= K + 1; i--)
            {
                P.Values[i + 1] = P.Values[i];
                P.Children[i + 1] = P.Children[i];
            }
            P.Values[K + 1] = x;
            P.Children[K + 1] = Xder;
            P.Cuenta = P.Cuenta + 1;
        }
        /// <summary>
        /// Método que Divide un nodo en nodos nuevos al momento que se llena un nodo
        /// </summary>
        /// <param name="x"></param>Clave a insertar
        /// <param name="Xder"></param>
        /// <param name="P"></param>Nodo sucesor
        /// <param name="K"></param> Posición donde debe insertarse la nueva clave
        /// <param name="Mda"></param>Clave que sube como nuevo padre de los nodos creados
        /// <param name="Mder"></param>Nuevo nodo donde van los valores mayores a la media
        private void DividirNodo(T x, BTNode<T> Xder, BTNode<T> P, int K, ref T Mda, ref BTNode<T> Mder)
        {
            int posmda;
            posmda = K <= Min ? Min : Min + 1;
            Mder = new BTNode<T>(); //nuevo nodo
            for (int i = posmda + 1; i < Orden; i++)
            {
                Mder.Values[i - posmda] = P.Values[i]; // es desplaza la mitad derecha del nuevo nodo, la clave mediana se queda en la izquierda
                Mder.Children[i - posmda] = P.Children[i];
            }
            Mder.Cuenta = Max - posmda; // claves en el nuevo nodo
            P.Cuenta = posmda; // claves que quedan en el nodo original
            //Inserción de X y la rama derecha
            if (K <= Orden / 2)
            {
                MeterHoja(x, Xder, P, K); // inserta nodo en izquierda
            }
            else
            {
                var newval = K - posmda;
                MeterHoja(x, Xder, Mder, newval);
            }//extrae mediana del nodo izquierdo
            Mda = P.Values[P.Cuenta];
            Mder.Children[0] = P.Children[P.Cuenta]; // Rama Inicial del nuevo nodo es la rama de la mediana
            P.Cuenta = P.Cuenta - 1; // disminuye por que se quitó el valor medio

        }
        /// <summary>
        /// Proceso en el que se baja hasta una rama vacía, se revisa si el nodo hoja tiene un espacio libre
        /// de ser así, se inserta el valor, de lo contrario, se divide el nodo actual y se crea uno nuevo, donde se reparten
        /// las claves entre el nodo original y el nuevo 
        /// </summary>
        /// <param name="Cl"></param> Clave con la que se trabaja a insertar
        /// <param name="R"></param> Nodo en el que se va evaluando el espacio
        /// <param name="EmpujarArriba"></param> Bandera que indica si el nodo se divide o no
        /// <param name="Mdna"></param> valor medio para la división del nodo
        /// <param name="Xr"></param> Nodo nuevo en caso de necesitarlo se crea para repartir las llaves
        private void Empujar(T Cl, BTNode<T> R, ref bool EmpujarArriba, ref T Mdna, ref BTNode<T> Xr)
        {
            var k = default(int);
            var esta = default(bool); // controla si ya ha sido insertado el valor

            if (ArbolVacio(R)) // termina la recursión
            {
                EmpujarArriba = true;
                Mdna = Cl;
                Xr = null;
            }
            else
            {
                BuscarNodo(Cl, R, ref esta, ref k); //K rama del árbol por donde se sigue la busqueda

                if (esta)
                {
                    return;
                }
                Empujar(Cl, R.Children[k], ref EmpujarArriba, ref Mdna, ref Xr);

                if (EmpujarArriba)
                {
                    if (R.Cuenta < Max) // no está lleno el nodo
                    {
                        EmpujarArriba = false; // termina el proceso de búsqueda
                        MeterHoja(Mdna, Xr, R, k); //Se inserta la clave
                    }
                    else
                    {
                        EmpujarArriba = true;
                        DividirNodo(Mdna, Xr, R, k, ref Mdna, ref Xr);
                    }
                }
            }
        }
        /// <summary>
        /// Proceso en el que se manipula la inserción de un nodo en el árbol
        /// </summary>
        /// <param name="Cl"></param> Clave o valor a ingresar
        /// <param name="root"></param> Nodo donde se inicia la evaluación para insertar
        private void Insertar(T Cl, ref BTNode<T> root)
        {
            var EmpujarArriba = default(bool);
            var Xr = default(BTNode<T>);
            var X = default(T);

            Empujar(Cl, root, ref EmpujarArriba, ref X, ref Xr);

            if (EmpujarArriba) // En caso la división de nodos llegue hasta la raíz, se crea un nuevo nodo y se cambia la raíz.
            {
                var P = new BTNode<T>();
                P.Cuenta = 1;
                P.Values[1] = X;
                P.Children[0] = root;
                P.Children[1] = Xr;
                root = P;
            }
        }
        /// <summary>
        /// Controla La inserción en el Árbol
        /// </summary>
        /// <param name="cl"></param>Valor a insertar
        /// <param name="root"></param> Raíz del arbol
        /// <returns></returns> retorna la nueva raíz del árbol
        public void Insertar(T cl)
        {
            var newroot = this.root;
            Insertar(cl, ref newroot);
            this.root = newroot;
        }

        #endregion

        #region Recorrido

        private void Inorder(BTNode<T> R)
        {
            if (!ArbolVacio(R))
            {
                Inorder(R.Children[0]);
                for (int i = 1; i <= R.Cuenta; i++)
                {
                    list.Value.Add(R.Values[i]);
                    Inorder(R.Children[i]);
                }
            }
        }

        public List<T> ToList()
        {
            list = new Lazy<List<T>>();
            Inorder(root);
            return list.Value;
        }
        #endregion

        #region Eliminación
        /// <summary>
        /// A partir de dos nodos, se combina y se crea un solo nodo
        /// Se ejecuta cuando se elimina y no se tiene la cantidad mínima de llaves
        /// </summary>
        /// <param name="P"></param>
        /// <param name="K"></param>
        private void Combina(BTNode<T> P, int K)
        {
            int J;
            BTNode<T> Q;

            Q = P.Children[K];
            P.Children[K - 1].Cuenta = P.Children[K - 1].Cuenta + 1;
            P.Children[K - 1].Values[P.Children[K - 1].Cuenta] = P.Values[K];
            P.Children[K - 1].Children[P.Children[K - 1].Cuenta] = Q.Children[0];

            for (J = 1; J <= Q.Cuenta; J++)
            {
                P.Children[K - 1].Cuenta = P.Children[K - 1].Cuenta + 1;
                P.Children[K - 1].Values[P.Children[K - 1].Cuenta] = Q.Values[J];
                P.Children[K - 1].Children[P.Children[K - 1].Cuenta] = Q.Children[J];
            }

            for (J = K; J <= P.Cuenta - 1; J++)
            {
                P.Values[J] = P.Values[J + 1];
                P.Children[J] = P.Children[J + 1];
            }
            P.Cuenta = P.Cuenta - 1;

        }
        /// <summary>
        /// Desciende la clave K = l del nodo padre P al hijo izquierdo y la inserta en la posición más alta, de esta forma se restablece el mínimo de claves
        /// Después sube la clave 1 del hermano derecho
        /// </summary>
        /// <param name="P"></param>Nodo que antecede al nodo que se está restaurando
        /// <param name="K"></param>Posición del nodo con menos claves que el mínimo
        private void MoverIzquierda(BTNode<T> P, int K)
        {
            //P.Children[k-1] nodo con menos claves que el mínimo
            P.Children[K - 1].Cuenta = P.Children[K - 1].Cuenta + 1;
            P.Children[K - 1].Values[P.Children[K - 1].Cuenta] = P.Values[K];
            P.Children[K - 1].Children[P.Children[K - 1].Cuenta] = P.Children[K].Children[0];

            P.Values[K] = P.Children[K].Values[1]; //sube al nodo padre la clave 1 del hermano derecho, sustituyendo a la que bajó
            P.Children[K].Children[0] = P.Children[K].Children[1];
            P.Children[K].Cuenta = P.Children[K].Cuenta - 1;

            for (int i = 1; i <= P.Children[K].Cuenta; i++)
            {
                P.Children[K].Values[i] = P.Children[K].Values[i + 1];
                P.Children[K].Children[i] = P.Children[K].Children[i + 1];
            }
        }
        /// <summary>
        /// En este procedimiento se deja a espacio en el nodo p.Childre[K] que es el nodo que tiene menos claves que el mínimo necesario , 
        /// inserta la clave k del nodo antecedente y a su vez asciende la clave mayor( la más a la derecha)del hermano izquierdo al nodo <paramref name="P"/>
        /// </summary>
        /// <param name="P"></param>Nodo que antecede al nodo que se está restaurando
        /// <param name="K"></param>Posición del nodo con menos claves que el mínimo
        private void MoverDerecha(BTNode<T> P, int K)
        {
            //P.Children[k] es nodo con menos claves que el mínimo
            for (int J = P.Children[K].Cuenta; J >= 1; J--)
            {
                P.Children[K].Values[J + 1] = P.Children[K].Values[J];
                P.Children[K].Children[J + 1] = P.Children[K].Children[J];
            }
            P.Children[K].Cuenta = P.Children[K].Cuenta + 1;
            P.Children[K].Children[1] = P.Children[K].Children[0];
            P.Children[K].Values[1] = P.Values[K]; // baja la clave del nodo padre
            //Ahora sube la clave desde el hermano izquierdo al nodo padre, para reemplazar la que antes bajó
            //{Hermano izquierdo P.Children[k-1]
            P.Values[K] = P.Children[K - 1].Values[P.Children[K - 1].Cuenta];
            P.Children[K].Children[0] = P.Children[K - 1].Children[P.Children[K - 1].Cuenta];
            P.Children[K - 1].Cuenta = P.Children[K - 1].Cuenta - 1;
        }

        /// <summary>
        /// Restaura el nodo P.Children[k] el cual se queda debajo del mínimo de claves 
        /// Si tiene hermano izquierdo siempre se trabaja con ese, en caso no tenga se pasa al hermano derecho
        /// En caso no hay claves se combinan los nodos
        /// </summary>
        /// <param name="P"></param> tiene la dirección del nodo antecedente del nodo P.Children[K] que se ha quedado con menos claves que el mínimo
        /// <param name="K"></param>
        private void Restablecer(BTNode<T> P, int K)
        {
            if (K > 0) //Tiene hermano izquierdo
            {
                if (P.Children[K - 1].Cuenta > Min) //Tiene más claves que el mínimo y por tanto puede desplazarse una clave
                {
                    MoverDerecha(P, K);
                }
                else
                {
                    Combina(P, K);
                }
            }
            else //solo tiene hermano derecho
            {
                if (P.Children[1].Cuenta > Min) //Tiene más claves que el mínimo
                {
                    MoverIzquierda(P, 1);
                }
                else
                {
                    Combina(P, 1);
                }
            }
        }
        /// <summary>
        /// En ete método se busca la clave inmediatamente sucesor de la clave K,  y ésta reemplaza a la clave K. 
        /// </summary>
        /// <param name="P"></param>Bidi donde se encuentra la clave k
        /// <param name="K"></param> Posición de la clave
        private void Sucesor(BTNode<T> P, int K)
        {
            BTNode<T> Q;

            Q = P.Children[K];
            while (Q.Children[0] != null)
            {
                Q = Q.Children[0];
                P.Values[K] = Q.Values[1];
            }
        }
        /// <summary>
        /// Elimina la clave junto con la rama que corresponde
        /// </summary>
        /// <param name="P"></param>Dirección del nodo
        /// <param name="K"></param>Posición de la clave a eliminar
        private void Quitar(BTNode<T> P, int K)
        {
            for (int j = K + 1; j <= P.Cuenta; j++)
            {
                P.Values[j - 1] = P.Values[j];//Desplaza una posición a la izquierda, con que elimina la referencia a la clave
                P.Children[j - 1] = P.Children[j];
            }
            P.Cuenta = P.Cuenta - 1;
        }
        /// <summary>
        /// Busca el nodo con la clave a eliminar, si el nodo es hoja se llama al método quitar
        /// En caso no sea hoja, se encuentra el sucesor inmediato de la clave, se coloca en el nodo donde está la clave, 
        /// después se elimina la clave sucesor en el nodo hoja
        /// </summary>
        /// <param name="Cl"></param>
        /// <param name="R"></param>
        /// <param name="Encontrado"></param>
        private void EliminarRegistro(T Cl, BTNode<T> R, ref bool Encontrado)
        {
            var k = 0;

            if (ArbolVacio(R))
            {
                Encontrado = false; // se ha recorrido todo el árbol
            }
            else
            {
                BuscarNodo(Cl, R, ref Encontrado, ref k);

                if (Encontrado)
                {
                    if (R.Children[k - 1] == null) //Las ramas están indexadas desde índice O a Max, por lo que este nodo es hoja
                    {
                        Quitar(R, k);
                    }
                    else // no es  hoja
                    {
                        Sucesor(R, k); //{Reemplaza R.claves[K] por su sucesor
                        EliminarRegistro(R.Values[k], R.Children[k], ref Encontrado);//{Elimina la clave sucesora en su nodo
                        if (!Encontrado)
                        {
                            return;
                        }
                    }
                }
                else //No ha sido localizada la clave
                {
                    EliminarRegistro(Cl, R.Children[k], ref Encontrado);
                    //se comprueba que el nodo hijo mantenga un número de claves igual o mayor que el mínimo necesario
                    if (R.Children[k] != null) //Condición de que no sea hoja
                    {
                        if (R.Children[k].Cuenta < Min)
                        {
                            Restablecer(R, k);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Este método se encarga de establecer la nueva raíz en caso se necesite
        /// </summary>
        /// <param name="cl"></param>
        /// <param name="root"></param>
        private void Eliminar(T cl, ref BTNode<T> root)
        {
            var Encontrado = false;
            BTNode<T> P;
            EliminarRegistro(cl, root, ref Encontrado);
            if (!Encontrado)
            {
                return;
            }
            else if (root.Cuenta == 0)
            {
                P = root;
                root = root.Children[0];
            }
        }

        public BTNode<T> Eliminar(T cl, BTNode<T> root)
        {
            var newroot = root;
            Eliminar(cl, ref newroot);

            return newroot;
        }
        #endregion




    }
}
