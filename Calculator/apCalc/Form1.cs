/*
 Keven Richard da Rocha Barreiros - 23143
 Victor Yuji Mimura               - 23158

https://learn.microsoft.com/en-us/dotnet/api/system.char.isdigit?view=net-8.0
https://stackoverflow.com/questions/2423377/what-is-the-invariant-culture
https://stackoverflow.com/questions/54010316/nullable-array-and-why-do-we-need-them
*/

using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace apCalc
{
    public partial class FrmCalculadora : Form
    {
        Pilha<double> pilhaDeNumeros = new Pilha<double>();

        public FrmCalculadora()
        {
            InitializeComponent();
        }

        private void FrmCalculadora_Load(object sender, EventArgs e)
        {
            
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            txtVisor.Text += '0';
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            txtVisor.Text += '1';
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            txtVisor.Text += '2';
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            txtVisor.Text += '3';
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            txtVisor.Text += '4';
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            txtVisor.Text += '5';
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            txtVisor.Text += '6';
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            txtVisor.Text += '7';
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            txtVisor.Text += '8';
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            txtVisor.Text += '9';
        }

        private void btnSoma_Click(object sender, EventArgs e)
        {
            txtVisor.Text += '+';
        }

        private void btnSub_Click(object sender, EventArgs e)
        {
            txtVisor.Text += '-';
        }

        private void btnMultiplicacao_Click(object sender, EventArgs e)
        {
            txtVisor.Text += '*';
        }

        private void btnDivisao_Click(object sender, EventArgs e)
        {
            txtVisor.Text += '/';
        }

        private void btnPotencia_Click(object sender, EventArgs e)
        {
            txtVisor.Text += '^';
        }

        private void btnPonto_Click(object sender, EventArgs e)
        {
            txtVisor.Text += '.';
        }

        private void btnAbreParenteses_Click(object sender, EventArgs e)
        {
            txtVisor.Text += '(';
        }

        private void btnFechaParenteses_Click(object sender, EventArgs e)
        {            
            txtVisor.Text += ')';
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            // limpamos os campos para uma próxima operação
            txtVisor.Text = "";
            txtResultado.Clear();
            lbSequencias.Text = string.Empty;
            txtVisor.Focus();
        }

        private void txtVisor_TextChanged(object sender, EventArgs e)
        {
            if(txtVisor.Text != "")
            {
                char caracterDigitado = txtVisor.Text[txtVisor.TextLength - 1];

                if (!EhCaracterPermitido(caracterDigitado))
                {
                    MessageBox.Show("Cadeia inválida!");
                    txtVisor.Text = txtVisor.Text.Substring(0, txtVisor.TextLength - 1);
                }

                else // se for caracter permitido, precisamos verificar se o caracter pode estar em sua respectiva posição
                {
                    if (!char.IsDigit(caracterDigitado)) // se for um operador, não pode estar em qualquer posição da expressão
                    {
                        if (txtVisor.TextLength == 1 && !"+-".Contains(caracterDigitado)) // se o operador é o único caracter digitado, ele não pode ser */^.
                        {
                            txtVisor.Text = txtVisor.Text.Substring(0, txtVisor.TextLength - 1);
                            MessageBox.Show("Caracter digitado inválido! Esse operador não pode ser digitado nessa posição da expressão!");
                        }

                        else if(txtVisor.TextLength > 1)
                        {
                            char caracterAnterior = txtVisor.Text[txtVisor.TextLength - 2];

                            if((caracterDigitado == '(' || caracterDigitado == ')') && caracterAnterior == '.')
                            {
                                txtVisor.Text = txtVisor.Text.Substring(0, txtVisor.TextLength - 1);
                                MessageBox.Show("Caracter digitado inválido! Esse operador não pode ser digitado nessa posição da expressão!");
                            }
                                

                            else if ((caracterDigitado == '*' || caracterDigitado == '/' || caracterDigitado == '^' || caracterDigitado == '.') && caracterAnterior == '(')
                            {
                                txtVisor.Text = txtVisor.Text.Substring(0, txtVisor.TextLength - 1);
                                MessageBox.Show("Caracter digitado inválido! Esse operador não pode ser digitado nessa posição da expressão!");
                            }

                            else if (EhOperador(caracterAnterior) && EhOperador(caracterDigitado))
                            {
                                txtVisor.Text = txtVisor.Text.Substring(0, txtVisor.TextLength - 1);
                                MessageBox.Show("Caracter digitado inválido! Esse operador não pode ser digitado nessa posição da expressão!");
                            }
                        }

                    }
                }
            }

        }

        private void btnIgual_Click(object sender, EventArgs e)
        {
            if (txtVisor.Text != "") // se há uma expressão para ser resolvida
            {
                if (!AnalisarBalanceamento()) // não está balanceada
                {
                    MessageBox.Show("A expressão digitada não está balanceada!");
                    txtVisor.Clear();
                }

                // se a expressão digitada está balanceada
                else
                {

                    double?[] numeros = new double?[txtVisor.Text.Length]; // vetor de double que permite null

                    int inicioNumero = -1; // variável indicadora do início de um número em uma string
                    int i = 0;             // contador do índice do vetor de números reais
                    int indexChar = -1;    // contador do índice da posição do char caracter na string txtVisor.Text
                    int tamanhoLeitura;

                    // adicionamos cada número da expressão no vetor de double
                    foreach (char caracter in txtVisor.Text)
                    {
                        indexChar++; // após todo char lido, o índice de onde o ponteiro está é atualizado

                        if (caracter == '(') // se for parênteses, ignora
                            continue;

                        else if (char.IsDigit(caracter) || caracter == '.') // se é número ou .
                        {
                            if (inicioNumero == -1)                    // se não há números em análise
                                inicioNumero = indexChar;              // indica o início do número a ser inserido no vetor
                        }

                        else                        // se é operador e
                            if (inicioNumero != -1) // se temos um número em análise
                            {
                                tamanhoLeitura = indexChar - inicioNumero; // variável indicadora do início da fatiação da string

                                // pega o número entre inicioNumero e i, o segundo parâmetro é usado para levar em consideração
                                //  o . como indicador decimal e não a , (visto que estamos no Brasil)
                                numeros[i] = double.Parse(txtVisor.Text.Substring(inicioNumero, tamanhoLeitura), CultureInfo.InvariantCulture);

                                i++;               // prepara para inserir o próximo número
                                inicioNumero = -1; // prepara para analisar outro número
                            }
                    }

                    if (inicioNumero != -1) // caso inicioNumero seja -1, então não deve-se inserir o elemento, visto que é um parênteses ou operador
                    {
                        tamanhoLeitura = indexChar + 1 - inicioNumero;
                        numeros[i] = double.Parse(txtVisor.Text.Substring(inicioNumero, tamanhoLeitura), CultureInfo.InvariantCulture);
                    }

                    char[] letras = new char[numeros.Length]; // criamos vetor de char para associar os números do vetor de double a letras
                    int j = 0;                                // contador do índice do vetor de letras

                    // criamos as letras para cada número
                    foreach (double? n in numeros)
                        if (n != null) // se há um número
                        {
                            letras[j] = (char)(65 + j); // vinculamos uma letra a ele
                            j++;                        // preparamos para vincular uma outra letra ao próximo número
                        }

                    //////////////////////////////////////////// CONVERTER PARA INFIXA ////////////////////////////////////////////
                    string cadeiaInfixa = ConverterExpressaoInfixa(numeros, letras);

                    //////////////////////////////////////////// CONVERTER PARA POSFIXA ////////////////////////////////////////////
                    string cadeiaPosFixa = ConverterInfixaPosfixa(cadeiaInfixa);

                    double ValorDaExpressao = ValorDaExpressaoPosfixa(cadeiaPosFixa, numeros);

                    // exibimos as sequências infixa e posfixa na tela
                    lbSequencias.Text += $"Cadeia infixa: {cadeiaInfixa} \n";
                    lbSequencias.Text += $"Cadeia posfixa: {cadeiaPosFixa} \n\n";
                    txtResultado.Text = ValorDaExpressao.ToString();
                }
            }
        }

        private string ConverterExpressaoInfixa(double?[] numeros, char[] letras)
        {
            string cadeia = "";
            int indexChar = -1;
            int inicioNumero = -1;

            foreach (char caracter in txtVisor.Text)
            {
                indexChar++; // após todo char lido, o índice de onde o ponteiro está é atualizado

                // se é número ou .
                if (char.IsDigit(caracter) || caracter == '.')
                {
                    if (inicioNumero == -1)                    // se não há números em análise
                        inicioNumero = indexChar;              // indica o início do número
                }

                // se for operador
                else
                {
                    if (caracter == '(')
                        cadeia += caracter;

                    else if (inicioNumero != -1) // se tem um número pra inserir
                    {
                        // definimos o número que terá de ser substituído
                        int tamanhoLeitura = indexChar - inicioNumero;
                        double numeroTrocar = double.Parse(txtVisor.Text.Substring(inicioNumero, tamanhoLeitura), CultureInfo.InvariantCulture);

                        // procuramos o índice do número no array de valores reais
                        int i = 0;

                        foreach (double n in numeros)
                        {
                            if (n == numeroTrocar)
                                break;
                            i++;
                        }

                        // adicionamos a respectiva letra à cadeia infixa
                        cadeia += letras[i] + $"{caracter}";

                        inicioNumero = -1; // prepara para analisar outro número
                    }

                    else if (cadeia[cadeia.Length - 1] == ')') // se o anterior foi parênteses, pode qualquer operação menor o .
                    {
                        if (caracter != '.')
                            cadeia += caracter;

                        else
                        {
                            MessageBox.Show("Não é possível resolver essa expressão! Tente novamente com uma expressão válida!");
                            txtVisor.Clear();
                            cadeia = "Inválida";
                            break;
                        }
                    }

                    // se o útltimo elemento foi um abre (, então não se pode usar * / ^
                    else if (cadeia.Length > 0 && cadeia[cadeia.Length - 1] == '(')
                    {
                        if (caracter == '+' || caracter == '-') // se é + -
                            cadeia += caracter;

                        else // se é * / ^ . , então a cadeia digitada está errada
                        {
                            MessageBox.Show("Não é possível resolver essa expressão! Tente novamente com uma expressão válida!");
                            txtVisor.Clear();
                            cadeia = "Inválida";
                            break;
                        }
                    }
                }
            }

            if (inicioNumero != -1) // caso necessário, insere o último número da cadeia
            {
                int tamanhoLeitura = indexChar + 1 - inicioNumero;
                double numeroTrocar = double.Parse(txtVisor.Text.Substring(inicioNumero, tamanhoLeitura), CultureInfo.InvariantCulture);

                int i = 0;

                foreach (double n in numeros)
                {
                    if (n == numeroTrocar)
                        break;
                    i++;
                }

                cadeia += letras[i];
            }

            return cadeia;
        }

        private bool EhCaracterPermitido(char c)
        {
            return "1234567890+-*/^().".Contains(c);
        }

        private bool EhOperador(char c)
        {
            return "+-*/^".Contains(c);

        }

        private bool AnalisarBalanceamento()
        {
            bool balanceada = true;
            string cadeia = txtVisor.Text;
            Pilha<char> pilha = new Pilha<char>(txtVisor.TextLength);

            for (int i = 0; i < cadeia.Length && balanceada; i++)
            {
                if (cadeia[i] == '(') // se é abre
                    pilha.Empilhar(cadeia[i]);

                else // se é fecha ou número
                {
                    try
                    {
                        if (!(cadeia[i] == ')')) // se é número / operador, ignoramos e passamos para o próximo caracter
                        {
                            continue;
                        }

                        else // se é fecha, desempilha um abre
                            pilha.Desempilhar();
                    }

                    catch // pilha ficou vazia
                    {
                        balanceada = false;
                    }
                }
            }

            // verifica se tiver abre na pilha, caso positivo, não está balanceada
            if (!pilha.EstaVazia)
                balanceada = false;

            return balanceada;
        }

        private string ConverterInfixaPosfixa(string cadeiaInfixa)
        {
            string resultado = "";
            Pilha<char> umaPilha = new Pilha<char>(); // Instancia a Pilha
            char operadorComMaiorPrecedencia;

            for (int indice = 0; indice < cadeiaInfixa.Length; indice++)
            {
                char simboloLido = cadeiaInfixa[indice];
                if (!EhOperador(simboloLido)
                    && simboloLido != '(' && simboloLido != ')') // se não é operador
                    resultado += simboloLido;

                else // é operador
                {
                    bool parar = false;

                    while (!parar && !umaPilha.EstaVazia && TerPrecedencia(umaPilha.OTopo(), simboloLido))
                    {
                        operadorComMaiorPrecedencia = umaPilha.Desempilhar();

                        if (operadorComMaiorPrecedencia != '(')
                            resultado += operadorComMaiorPrecedencia;

                        else
                            parar = true;
                    }

                    if (simboloLido != ')')
                        umaPilha.Empilhar(simboloLido);

                    else // fará isso quando pilha[topo] = '('
                        operadorComMaiorPrecedencia = umaPilha.Desempilhar();
                }
            }

            while (!umaPilha.EstaVazia) // descarrega a pilha para a saída
            {
                operadorComMaiorPrecedencia = umaPilha.Desempilhar();

                if (operadorComMaiorPrecedencia != '(')
                    resultado += operadorComMaiorPrecedencia;
            }

            return resultado;
        }

        private bool TerPrecedencia(char A, char B)
        {
            switch (A) // verificaremos a precedência de acordo com o operador do topo da pilha
            {
                case '(':
                case ')':
                    return false;

                case '+':
                case '-':
                    if (B == '+' || B == '-' || B == ')')
                        return true;

                    else
                        return false;

                case '*':
                case '/':
                case '^':
                    if (B == '+' || B == '-' || B == ')' || B == '*' || B == '/')
                        return true;

                    else
                        return false;

                default: return false;
            }
        }
        private double ValorDaExpressaoPosfixa(string cadeiaPosfixa, double?[] numeros)
        {
            var umaPilha = new Pilha<double>();
            int numIndex = 0;
            for (int atual = 0; atual < cadeiaPosfixa.Length; atual++)
            {
                char simbolo = cadeiaPosfixa[atual];
                if (!EhOperador(simbolo) && numeros!=null) // É Operando 
                    umaPilha.Empilhar((double)numeros[numIndex++]);

                else
                {
                    double operando2 = umaPilha.Desempilhar();
                    double operando1 = umaPilha.Desempilhar();
                    double valorParcial = ValorDaSubExpressao(operando1, simbolo, operando2);
                    umaPilha.Empilhar(valorParcial);
                }
            }
            return umaPilha.Desempilhar(); // resultado final
        }
        private double ValorDaSubExpressao(double operando1, char simbolo, double operando2)
        {
            switch (simbolo)
            {
                case '+':
                    return operando1 + operando2;
                    
                case '-':
                    return operando1 - operando2;
                    
                case '*':
                    return operando1 * operando2;
                    
                case '/':
                    return operando1 / operando2;
                    
                case '^':
                    return Math.Pow(operando1, operando2);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
