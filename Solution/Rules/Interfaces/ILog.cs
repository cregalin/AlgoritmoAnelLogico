using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rules.Interfaces
{
    public interface ILog
    {
        Stopwatch Timer { get; set; }

        void novoCoordenador(long id);

        void requisicaoRecebida(long id);

        void requisicaoTratada(long id);

        void erroAoTratarRequisicao(long id, Exception ex);

        void eleicaoIniciada(long id);

        void processosAtivos();

        void eleicaoTerminada(long id);

        void demaisProcessosComoNaoCoordenadores();

        void erroAoAtualizarOCoorenador(Exception ex);

        void processoCriado(long id);

        void erroAoCriarNovoProceso(Exception ex);

        void erroAoExecutarRequisicao(Exception ex);

        void processoFezUmaRequisicao(long id);

        void naoFoiObtidaNenhumaRespostaParaARequisicao();

        void erroAoInativarCoordenador(Exception ex);

        void erroAoInativarProcesso(Exception ex);

        void processoInativado(long id);
    }
}
