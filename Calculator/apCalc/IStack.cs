/*
 Keven Richard da Rocha Barreiros - 23143
 Victor Yuji Mimura               - 23158
*/

using System;
using System.Collections.Generic;

public interface IStack<Dados>
{
    void Empilhar(Dados umDado);
    Dados Desempilhar();
    Dados OTopo();
    int Tamanho { get; }
    bool EstaVazia { get; }
    List<Dados> Conteudo();
}
