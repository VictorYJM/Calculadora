using System;
using System.Collections.Generic;

public class Pilha<Dados> : IStack<Dados>
{
    const int MAXIMO = 1000;
    int topo;
    int maximo;
    Dados[] p;

    public Pilha(int tamanhoDesejado)
    {
        topo = -1;              // topo ainda não existe na pilha
        p = new Dados[tamanhoDesejado];
        maximo = tamanhoDesejado; // tamanho físico

    }

    public Pilha() : this(MAXIMO) { }

    public int Tamanho => topo + 1;

    public bool EstaVazia => topo < 0;

    public List<Dados> Conteudo(bool semIndices)
    {
        List<Dados> saida = new List<Dados>();

        Pilha<Dados> aux = new Pilha<Dados>(maximo);

        // enquanto há dados na nossa pilha
        while (!this.EstaVazia)
        {
            saida.Add(this.OTopo());          // adiciona o item do topo à pilha de dados aux
            aux.Empilhar(this.Desempilhar()); // desempilha o item já empilhado em aux
        }

        // reeconstroi a pilha de dados
        while (!aux.EstaVazia)
            this.Empilhar(aux.Desempilhar());

        return saida;
    }

    public List<Dados> Conteudo()
    {
        List<Dados> saida = new List<Dados>();
        for (int i = topo; i >= 0; i--)
            saida.Add(p[i]);

        return saida;
    }

    // retira um dado da pilha de dados
    public Dados Desempilhar()
    {
        if (topo < 0)
            throw new Exception("Underflow de pilhha!");

        // desempilhado recebe p[topo], em seguida topo é decrementado para atualizar seu estado
        // Dados desempilhado = p[topo--]

        Dados desempilhado = p[topo];
        p[topo--] = default(Dados); // libera memória
        return desempilhado;
    }

    // insere um dado genérico na pilha de dados
    public void Empilhar(Dados umDado)
    {
        if (topo >= maximo)
            throw new Exception("Overflow de pilha!");

        // incrementa topo para que atualize seu estado antes da inserção, evitando o erro de inserir na oposição -1
        p[++topo] = umDado;
    }

    // apenas retorna o dado que está no topo da pilha
    public Dados OTopo()
    {
        if (topo < 0)
            throw new Exception("Underflow de pihlha!");

        return p[topo];
    }
}