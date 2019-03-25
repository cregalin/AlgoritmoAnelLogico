using Rules.Enums;
using Rules.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rules.Utils
{
    public class Log : ILog
    {
        private Stopwatch intervalo = new Stopwatch();
        public Stopwatch Timer { get; set; } = new Stopwatch();

        private readonly string _separator = new string('-', 70);

        private void writeLine(string value)
        {
            Console.WriteLine();
            Console.WriteLine(value);
            Console.WriteLine(_separator + $"{intervalo.ElapsedMilliseconds.ToString()}ms - {Timer.ElapsedMilliseconds.ToString()}ms");
            resetWatch();
        }

        private void resetWatch()
        {
            if (intervalo.IsRunning)
                intervalo.Stop();
            intervalo.Reset();
            intervalo.Start();
        }

        public void novoCoordenador(long id) =>
            writeLine($"Processo {id} definido como coordenador.");

        public void requisicaoRecebida(long id) =>
            writeLine($"Requisição do processo {id} recebida pelo coordenador ({Ring.ActiveProcedures.RetrieveManager().Identifier}).");

        public void requisicaoTratada(long id) =>
            writeLine($"Requisição do processo {id} tratada.");

        public void erroAoTratarRequisicao(long id, Exception ex) =>
            writeLine($"Erro ao tratar requisição do processo {id}: {ex.Message}");

        public void eleicaoIniciada(long id) =>
            writeLine($"Eleição iniciada pelo processo {id}");

        public void processosAtivos() =>
            writeLine($"{Ring.ActiveProcedures.Count} processos ativos");

        public void eleicaoTerminada(long id) =>
            writeLine($"Eleição terminada, o processo {id} é o novo coordenador.");

        public void demaisProcessosComoNaoCoordenadores() =>
            writeLine("Demais processos marcados como não-coordenadores.");

        public void erroAoAtualizarOCoorenador(Exception ex) =>
            writeLine($"Erro ao atualizar coordenador: {ex.Message}");

        public void processoCriado(long id) =>
            writeLine($"Processo {id} criado.");

        public void erroAoCriarNovoProceso(Exception ex) =>
            writeLine($"Erro ao criar novo processo: {ex.Message}");

        public void erroAoExecutarRequisicao(Exception ex) =>
            writeLine($"Erro ao executar requisição: {ex.Message}");

        public void processoFezUmaRequisicao(long id) =>
            writeLine($"Processo {id} fez uma requisição.");

        public void naoFoiObtidaNenhumaRespostaParaARequisicao() =>
            writeLine($"Não foi obtida nenhuma resposta para a requisição.");

        public void erroAoInativarCoordenador(Exception ex) =>
            writeLine($"Erro ao inativar coordenador: {ex.Message}");

        public void erroAoInativarProcesso(Exception ex) =>
            writeLine($"Erro ao inativar processo: {ex.Message}");

        public void processoInativado(long id) =>
            writeLine($"Processo {id} inativado.");
        public void error(Exception ex, ProcessType processType) =>
            writeLine($"Erro ao {processType.Value}: {ex.Message}");
    }
}
