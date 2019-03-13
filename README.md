# Algoritmo de Eleição - Anel lógico
Muitos algoritmos distribuídos requerem que um processo aja como coordenador.
Existem algumas formas para se eleger quem é o coordenador e em geral os algoritmos de eleição tentam  localizar o processo que tenha o identificador mais alto, sendo este escolhido como coordenador.
O algoritmo de anel lógico, define quem é o coordenador, da seguinte forma: quando qualquer processo nota que o coordenador não está funcionando, monta uma mensagem eleição, com o seu próprio número de processo e envia ao seu sucessor.
Caso o sucessor tenha caído, o remetente segue até achar o próximo processo em funcionamento.
Ao retornar a origem, líder é definido( maior de todos os processos na lista) e mensagem COORNEADOR é enviada, com o novo líder e a lista de processos em funcionamento.
A lógica do funcionamento é a seguinte:
1) P envia mensagem de eleição com seu PID
2) Sucessor recebe mensagem, adiciona seu PID e passa o próximo
3) Quando voltar a P, a mensaem m muda para coordenador e volta a circular no anel
4) Cada um assume que o coordenador é o maior PID da lista circulante, e a lista contém os PIDs ativos.

