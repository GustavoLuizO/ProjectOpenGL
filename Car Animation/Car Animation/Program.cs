using System;
using Tao.FreeGlut;
using Tao.OpenGl;

namespace Car_Animation
{
    //Vou separar cada coisa em uma function Luquinhas, pra ficar facil entender depois
    internal class Program
    {
        static float rot = 0.0f;
        static float carPositionX = 0.0f;
        const float PI = 3.14f;
        static int acompanhamentoNumPassos = 0;
        static int horario = 0;
        static Cor CorFundo = new Cor(r: 0.086f, g: 0.447f, b: 0.698f);
        static Cor CorCarro = new Cor(r: 1.0f, g: 0f, b: 0f);
        static Cor CorTetoCarro = new Cor(r: 0.7f, g: 0f, b: 0f);
        static float posicaoNuvens1 = 0.0f;
        static float posicaoNuvens2 = 0.5f;
        static float posicaoNuvens3 = -0.6f;
        static float posicaoNuvens4 = -0.3f;
        static float posicaoNuvens5 = 0.4f;
        static float posicaoNuvens6 = 0.7f;
        const int width = 1200;
        const int height = 800;
        static bool isCloudsAvailable = true;
        static bool isCarOn = true;
        static bool isRedLightOn = false;
        static bool isYellowLightOn = false;
        static bool isGreenLightOn = true;

        static void Main(string[] args)
        {
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_DOUBLE | Glut.GLUT_RGB);
            Glut.glutInitWindowSize(width, height);
            Glut.glutCreateWindow("Carro");
            Glut.glutSpecialFunc(TeclasEspeciais);
            Glut.glutDisplayFunc(display);
            Glut.glutTimerFunc(40, Timer, 1);
            Glut.glutTimerFunc(40, TimerNuvem, 1);
            CriarMenu();
            inicializa();
            Glut.glutMainLoop();
        }

        static void inicializa()
        {
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            Glu.gluOrtho2D(0.0, 1.0, 0.0, 1.0);
            Gl.glClearColor(1, 1, 1, 0);
        }

        static void display()
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);

            rot -= 0.8f;

            fundo(CorFundo);

            if (isCloudsAvailable)
                DesenharVariasNuvens();

            desenhaRoda(true);
            desenhaRoda(false);
            desenhaCarro();

            desenharSemaforo();

            Glut.glutSwapBuffers();
            Glut.glutPostRedisplay();
        }


        static void desenhaRoda(bool isFrontWheel)
        {
            float positionX = carPositionX;
            if (isFrontWheel)
                positionX += 0.1f;
            else
                positionX += -0.1f;

            Gl.glPushMatrix();
            Gl.glTranslatef(positionX, 0.15f, 0.1f);
            if (isCarOn)
            {
                Gl.glRotatef(rot, 0.0f, 0.0f, 1.0f);
            }

            Gl.glScalef(0.3f, 0.3f, 0.3f);

            // pneu
            float raio = 0.1f;
            int numSegments = 30;
            Gl.glColor3f(0.0f, 0.0f, 0.3f);
            Gl.glBegin(Gl.GL_POLYGON);
            for (int i = 0; i < numSegments; i++)
            {
                float angle = (float)(2 * PI * i / numSegments);
                float x = (float)(raio * Math.Cos(angle));
                float y = (float)(raio * Math.Sin(angle));
                Gl.glVertex2f(x, y);
            }
            Gl.glEnd();

            // aro
            raio = 0.05f;
            Gl.glColor3f(0.7f, 0.7f, 0.7f);
            Gl.glBegin(Gl.GL_POLYGON);
            for (int i = 0; i < numSegments; i++)
            {
                float angle = (float)(2 * PI * i / numSegments);
                float x = (float)(raio * Math.Cos(angle));
                float y = (float)(raio * Math.Sin(angle));
                Gl.glVertex2f(x, y);
            }
            Gl.glEnd();

            int numLines = 8;
            Gl.glColor3f(0.3f, 0.3f, 0.3f);
            Gl.glBegin(Gl.GL_LINES);
            for (int i = 0; i < numLines; i++)
            {
                float angle = (float)(2 * PI * i / numLines);
                float x1 = (float)(raio * Math.Cos(angle));
                float y1 = (float)(raio * Math.Sin(angle));
                float x2 = (float)((raio * 1.75) * Math.Cos(angle));
                float y2 = (float)((raio * 1.75) * Math.Sin(angle));
                Gl.glVertex2f(x1, y1);
                Gl.glVertex2f(x2, y2);
            }
            Gl.glEnd();

            Gl.glPopMatrix();
        }

        static void desenhaCarro()
        {

            Cor corpo = CorCarro;
            Cor teto = CorTetoCarro;
            Gl.glPushMatrix();
            Gl.glTranslatef(carPositionX, 0.15f, 0.0f);
            Gl.glScalef(0.2f, 0.2f, 0.2f);

            // corpo
            Gl.glColor3f(corpo.R, corpo.G, corpo.B);
            Gl.glPolygonMode(Gl.GL_FRONT_AND_BACK, Gl.GL_FILL);
            Gl.glBegin(Gl.GL_POLYGON);
            Gl.glVertex2f(-0.6f, 0.1f);
            Gl.glVertex2f(0.6f, 0.1f);
            Gl.glVertex2f(0.6f, 0.2f);
            Gl.glVertex2f(0.4f, 0.35f);
            Gl.glVertex2f(-0.4f, 0.35f);
            Gl.glVertex2f(-0.6f, 0.2f);
            Gl.glEnd();

            // teto
            Gl.glColor3f(teto.R, teto.G, teto.B);
            Gl.glPolygonMode(Gl.GL_FRONT_AND_BACK, Gl.GL_FILL);
            Gl.glBegin(Gl.GL_POLYGON);
            Gl.glVertex2f(-0.4f, 0.35f);
            Gl.glVertex2f(0.4f, 0.35f);
            Gl.glVertex2f(0.3f, 0.5f);
            Gl.glVertex2f(-0.3f, 0.5f);
            Gl.glEnd();

            // janela
            Gl.glColor3f(1.0f, 1.0f, 1.0f);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glVertex2f(0.0f, 0.4f);
            Gl.glVertex2f(0.3f, 0.4f);
            Gl.glVertex2f(0.3f, 0.47f);
            Gl.glVertex2f(0.0f, 0.47f);

            Gl.glEnd();
            Gl.glPopMatrix();
        }

        static void TeclasEspeciais(int key, int x, int y)
        {
            switch (key)
            {
                case Glut.GLUT_KEY_LEFT:
                    if (carPositionX > 0.1f && isCarOn)
                    {
                        rot += 10.0f;
                        carPositionX -= 0.05f;
                    }
                    break;

                case Glut.GLUT_KEY_RIGHT:
                    if (carPositionX < 0.9f && isCarOn)
                    {
                        rot -= 10.0f;
                        carPositionX += 0.05f;
                    }
                    break;
            }

            Glut.glutPostRedisplay();
        }

        static void fundo(Cor Cor)
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
            ConfigurarHorarioDia();

            DesenharRodovia();
        }

        static void TimerNuvem(int value)
        {
            AlterandoPosicaoNuvens();

            Glut.glutPostRedisplay();
            Glut.glutTimerFunc(10, TimerNuvem, 1);
        }

        static void Timer(int value)
        {
            const int numPassos = 100; // número de passos para a transição completa
            const int numHorarios = 3; // número de horários (dia, tarde, noite)

            float progresso = (float)acompanhamentoNumPassos / numPassos; // progresso da transição, entre 0 e 1

            Cor corAnterior = CorFundo;
            Cor corNova = null;

            switch (horario)
            {
                case 0: // Dia para tarde
                    if (acompanhamentoNumPassos < numPassos / 2)
                        corNova = LerpCor(corAnterior, new Cor(r: 0.0f, g: 0.74902f, b: 1.0f), progresso);
                    else
                        corNova = LerpCor(corAnterior, new Cor(r: 0.086f, g: 0.447f, b: 0.698f), progresso);
                    break;
                case 1: // Tarde para noite
                    corNova = LerpCor(corAnterior, new Cor(r: 0.086f, g: 0.447f, b: 0.698f), progresso);
                    break;
                case 2: // Noite para dia
                    corNova = LerpCor(corAnterior, new Cor(r: 0.0f, g: 0.0f, b: 0.0f), progresso);
                    break;
            }

            if (corNova != null)
            {
                CorFundo = corNova;
                fundo(CorFundo);
                Glut.glutPostRedisplay();
            }

            acompanhamentoNumPassos++;
            if (acompanhamentoNumPassos >= numPassos)
            {
                acompanhamentoNumPassos = 0;
                horario = (horario + 1) % numHorarios;
            }

            Glut.glutTimerFunc(50, Timer, 1);
        }
        static Cor LerpCor(Cor CorA, Cor CorB, float progesso)
        {
            float r = (1 - progesso) * CorA.R + progesso * CorB.R;
            float g = (1 - progesso) * CorA.G + progesso * CorB.G;
            float b = (1 - progesso) * CorA.B + progesso * CorB.B;
            return new Cor(r, g, b);
        }

        public class Cor
        {
            public Cor(float r, float g, float b)
            {
                R = r;
                G = g;
                B = b;
            }

            public float R { get; set; }
            public float G { get; set; }
            public float B { get; set; }
        }

        static void AlterandoPosicaoNuvens()
        {
            posicaoNuvens1 += 0.001f;
            if (posicaoNuvens1 > 1.0f)
            {
                posicaoNuvens1 = -0.5f;
            }

            posicaoNuvens2 += 0.0015f;
            if (posicaoNuvens2 > 1.5f)
            {
                posicaoNuvens2 = -0.5f;
            }

            posicaoNuvens3 += 0.0012f;
            if (posicaoNuvens3 > 1.2f)
            {
                posicaoNuvens3 = -0.4f;
            }

            posicaoNuvens4 += 0.0013f;
            if (posicaoNuvens4 > 1.3f)
            {
                posicaoNuvens4 = -0.3f;
            }

            posicaoNuvens5 += 0.0014f;
            if (posicaoNuvens5 > 1.4f)
            {
                posicaoNuvens5 = -0.2f;
            }

            posicaoNuvens6 += 0.0016f;
            if (posicaoNuvens6 > 1.6f)
            {
                posicaoNuvens6 = -0.6f;
            }
        }

        static void DesenharVariasNuvens()
        {
            Gl.glPushMatrix();
            Gl.glTranslatef(posicaoNuvens1, 0.8f, 0.0f);
            Gl.glScalef(0.2f, 0.2f, 1.0f);
            desenhaNuvem();
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslatef(posicaoNuvens2, 0.7f, 0.0f);
            Gl.glScalef(0.15f, 0.15f, 1.0f);
            desenhaNuvem();
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslatef(posicaoNuvens3, 0.9f, 0.0f);
            Gl.glScalef(0.25f, 0.25f, 1.0f);
            desenhaNuvem();
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslatef(posicaoNuvens4, 0.7f, 0.0f);
            Gl.glScalef(0.2f, 0.2f, 1.0f);
            desenhaNuvem();
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslatef(posicaoNuvens5, 0.85f, 0.0f);
            Gl.glScalef(0.3f, 0.3f, 1.0f);
            desenhaNuvem();
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslatef(posicaoNuvens6, 0.75f, 0.0f);
            Gl.glScalef(0.25f, 0.25f, 1.0f);
            desenhaNuvem();
            Gl.glPopMatrix();
        }

        static void ConfigurarHorarioDia()
        {
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glColor3f(0.282353f, 0.239216f, 0.545098f);
            Gl.glVertex2f(0, height); // Top-left
            Gl.glVertex2f(width, height); // Top-right

            Gl.glColor3f(CorFundo.R, CorFundo.G, CorFundo.B);
            Gl.glVertex2f(width, 0); // Bottom-right
            Gl.glVertex2f(0, 0); // Bottom-left
            Gl.glEnd();

            configurarAstroCeu();
        }

        static void DesenharRodovia()
        {
            int windowWidth = width;
            int windowHeight = height;

            float relativeWidth = (float)windowWidth / (float)windowHeight;

            Gl.glBegin(Gl.GL_QUADS);
            Gl.glColor3f(0.0f, 0.0f, 0.0f);
            Gl.glVertex2f(relativeWidth, 0.25f);
            Gl.glVertex2f(0.0f, 0.25f);
            Gl.glVertex2f(0.0f, 0.0f);
            Gl.glVertex2f(relativeWidth, 0.0f);

            Gl.glColor3f(1.0f, 1.0f, 0.0f); // amarelo

            float lineWidth = 0.11f * relativeWidth; // largura da linha
            float gapWidth = 0.05f * relativeWidth; // largura do espaço entre as linhas
            int numLines = (int)(7 * relativeWidth); // número de linhas que serão desenhadas

            // loop para criar as linhas tracejadas
            for (int i = 0; i < numLines; i++)
            {
                float startX = (i * (lineWidth + gapWidth)); // posição inicial da linha
                float endX = startX + lineWidth; // posição final da linha

                // desenha a linha amarela
                Gl.glVertex2f(startX + lineWidth / 2, 0.125f);
                Gl.glVertex2f(startX, 0.125f);
                Gl.glVertex2f(startX, 0.1f);
                Gl.glVertex2f(startX + lineWidth / 2, 0.1f);

                // desenha o espaço entre as linhas
                Gl.glColor3f(0.0f, 0.0f, 0.0f);
                Gl.glVertex2f(endX, 0.125f);
                Gl.glVertex2f(endX - gapWidth / 2, 0.125f);
                Gl.glVertex2f(endX - gapWidth / 2, 0.1f);
                Gl.glVertex2f(endX, 0.1f);

                // volta para a cor amarela para desenhar a próxima linha
                Gl.glColor3f(1.0f, 1.0f, 0.0f);
            }

            Gl.glEnd();
        }
        static void configurarAstroCeu()
        {
            if (horario == 1 || horario == 2)
                Gl.glColor3f(0.8f, 0.8f, 0.8f); //SOL - AMARELO
            else
                Gl.glColor3f(1.0f, 1.0f, 0.0f); //LUA - CINZA CLARO

            Gl.glPushMatrix();
            Gl.glTranslatef(0.5f, 0.9f, 0.0f);
            Glu.gluDisk(Glu.gluNewQuadric(), 0, 0.15f, 32, 1); //Pelo que entendi, desenha um circulo com raio de 0.1f e 32 de segmento
            Gl.glPopMatrix();
        }


        static void desenhaNuvem()
        {
            Gl.glColor3f(1.0f, 1.0f, 1.0f);

            //Faço 4 circulos aqui praticamentes juntos mas separados na localidades. 
            Gl.glPushMatrix();
            Gl.glTranslatef(-0.2f, 0.2f, 0.0f);
            Glut.glutSolidSphere(0.15f, 20, 20);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslatef(-0.1f, 0.3f, 0.0f);
            Glut.glutSolidSphere(0.2f, 20, 20);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslatef(0.1f, 0.3f, 0.0f);
            Glut.glutSolidSphere(0.2f, 20, 20);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslatef(0.2f, 0.2f, 0.0f);
            Glut.glutSolidSphere(0.15f, 20, 20);
            Gl.glPopMatrix();
        }

        private static void MenuSwitch(int value)
        {
            switch (value)
            {
                case 1:
                    CorCarro = new Cor(r: 0.0f, g: 0.0f, b: 1.0f);
                    CorTetoCarro = new Cor(r: 0.0f, g: 0.0f, b: 0.7f);
                    break;
                case 2:
                    CorCarro = new Cor(r: 0.0f, g: 1.0f, b: 0.0f);
                    CorTetoCarro = new Cor(r: 0.0f, g: 0.7f, b: 0.0f);
                    break;
                case 3:
                    CorCarro = new Cor(r: 1.0f, g: 0.0f, b: 0.0f);
                    CorTetoCarro = new Cor(r: 0.7f, g: 0.0f, b: 0.0f);
                    break;
                case 4:
                    isCloudsAvailable = !isCloudsAvailable;
                    break;
                case 5:
                    isCarOn = !isCarOn;
                    break;
                case 6:
                    ligarSemaforoVermelho();
                    break;
                case 7:
                    ligarSemaforoAmarelo();
                    break;
                case 8:
                    ligarSemaforoVerde();
                    break;
            }
        }

        private static void ligarSemaforoVermelho()
        {
            isRedLightOn = true;
            isYellowLightOn = false;
            isGreenLightOn = false;
            isCarOn = false;
        }
        private static void ligarSemaforoAmarelo()
        {
            isRedLightOn = false;
            isYellowLightOn = true;
            isGreenLightOn = false;
            isCarOn = false;
        }
        private static void ligarSemaforoVerde()
        {
            isRedLightOn = false;
            isYellowLightOn = false;
            isGreenLightOn = true;
            isCarOn = true;
        }

        private static void CriarMenu()
        {
            int menu = Glut.glutCreateMenu(MenuSwitch);
            Glut.glutAddMenuEntry("Trocar cor do carro para azul", 1);
            Glut.glutAddMenuEntry("Trocar cor do carro para verde", 2);
            Glut.glutAddMenuEntry("Trocar cor do carro para vermelho", 3);
            Glut.glutAddMenuEntry("Remover nuvens", 4);
            Glut.glutAddMenuEntry("Desligar/Ligar carro", 5);
            Glut.glutAddMenuEntry("Semaforo vermelho", 6);
            Glut.glutAddMenuEntry("Semaforo amarelo", 7);
            Glut.glutAddMenuEntry("Semaforo verde", 8);
            Glut.glutAttachMenu(Glut.GLUT_RIGHT_BUTTON);
        }

        private static void desenharRetangulo(float x, float y, float width, float height)
        {
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glVertex2f(x, y);
            Gl.glVertex2f(x + width, y);
            Gl.glVertex2f(x + width, y + height);
            Gl.glVertex2f(x, y + height);
            Gl.glEnd();
        }

        private static void desenharCirculo(float x, float y, float radius)
        {
            int segments = 30;
            Gl.glBegin(Gl.GL_TRIANGLE_FAN);
            for (int i = 0; i <= segments; i++)
            {
                float angle = (float)(i * 2 * Math.PI / segments);
                float xPos = x + (float)Math.Cos(angle) * radius;
                float yPos = y + (float)Math.Sin(angle) * radius;
                Gl.glVertex2f(xPos, yPos);
            }
            Gl.glEnd();
        }

        private static void desenharSemaforo()
        {
            Gl.glPushMatrix();
            Gl.glScalef(0.3f, 0.3f, 0.3f);
            Gl.glTranslatef(3f, 2f, 0.0f);
            Gl.glColor3f(0.0f, 0.0f, 0.0f);
            desenharRetangulo(-0.2f, -0.6f, 0.4f, 1.2f);

            // Luz vermelha
            if (isRedLightOn)
            {
                Gl.glColor3f(1.0f, 0.0f, 0.0f);
            }

            if (!isRedLightOn)
            {
                Gl.glColor3f(0.1f, 0.0f, 0.0f);
            }
            desenharCirculo(-0.0f, 0.4f, 0.15f);

            // Luz amarela
            if (isYellowLightOn)
            {
                Gl.glColor3f(1.0f, 1.0f, 0.0f);
            }

            if (!isYellowLightOn)
            {
                Gl.glColor3f(0.1f, 0.1f, 0.0f);
            }

            desenharCirculo(-0.0f, 0.0f, 0.15f);

            // Luz verde
            if (isGreenLightOn)
            {
                Gl.glColor3f(0.0f, 1.0f, 0.0f);
            }

            if (!isGreenLightOn)
            {
                Gl.glColor3f(0.0f, 0.1f, 0.0f);
            }

            desenharCirculo(-0.0f, -0.4f, 0.15f);
            Gl.glPopMatrix();

        }
    }
}
