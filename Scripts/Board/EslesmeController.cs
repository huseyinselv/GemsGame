using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EslesmeController : MonoBehaviour
{
    Board board;

    public List<Mucevher> BulunanMucevherListe = new List<Mucevher>();

    private void Awake()
    {
        board = Object.FindObjectOfType<Board>(); 
    }

    public void EslesmeleriBulFNC()
    {

        BulunanMucevherListe.Clear();

        for (int x = 0; x < board.genislik; x++)
        {
            for (int y = 0; y < board.yuseklik; y++)
            {
                Mucevher gecerliMucevher = board.tumMucevherler[x, y];

                if(gecerliMucevher != null)
                {
                    // x sýrasýndaki eþleþmeleri kontrol edildi
                    if(x>0 && x<board.genislik-1)
                    {
                        Mucevher solMucevher = board.tumMucevherler[x-1, y];
                        Mucevher sagMucevher = board.tumMucevherler[x+1, y];

                        if(solMucevher != null && sagMucevher != null)
                        {
                            if(solMucevher.tipi == gecerliMucevher.tipi && sagMucevher.tipi == gecerliMucevher.tipi)
                            {
                                gecerliMucevher.eslestimi = true;
                                solMucevher.eslestimi = true;
                                sagMucevher.eslestimi = true;
                                BulunanMucevherListe.Add(gecerliMucevher);
                                BulunanMucevherListe.Add(solMucevher);
                                BulunanMucevherListe.Add(sagMucevher);
                            }
                        }

                    }
                    // y sýrasýndkai eþleþtirmeler kontrol edildi
                    if (y > 0 && y < board.yuseklik - 1)
                    {
                        Mucevher altMucevher = board.tumMucevherler[x , y-1];
                        Mucevher ustMucevher = board.tumMucevherler[x, y+1];

                        if (altMucevher != null && ustMucevher != null)
                        {
                            if (altMucevher.tipi == gecerliMucevher.tipi && ustMucevher.tipi == gecerliMucevher.tipi)
                            {
                                gecerliMucevher.eslestimi = true;
                                altMucevher.eslestimi = true;
                                ustMucevher.eslestimi = true;
                                BulunanMucevherListe.Add(gecerliMucevher);
                                BulunanMucevherListe.Add(altMucevher);
                                BulunanMucevherListe.Add(ustMucevher);
                            }
                        }

                    }
                }
            }
        } // döngüler bitti

        if(BulunanMucevherListe.Count > 0)
        {
            BulunanMucevherListe=BulunanMucevherListe.Distinct().ToList();
        }
        bombayiBulFNC();
    }

    //BOMBAYI BULDURUYORUZ
    public void bombayiBulFNC()
    {
        for (int i = 0; i < BulunanMucevherListe.Count; i++)
        {
            Mucevher mucevher = BulunanMucevherListe[i];
            int x = mucevher.posIndex.x;
            int y = mucevher.posIndex.y;

            if(mucevher.posIndex.x > 0)
            {
                if (board.tumMucevherler[x-1,y] !=null) 
                {
                    if (board.tumMucevherler[x - 1, y].tipi == Mucevher.MucevherTipi.bomba)
                    {
                       bombaBolgesiniIsaretle(new Vector2Int(x-1,y), board.tumMucevherler[x-1 , y] );
                    }
                }
            }

            if (mucevher.posIndex.x < board.genislik-1)
            {
                if (board.tumMucevherler[x + 1, y] != null)
                {
                    if (board.tumMucevherler[x + 1, y].tipi == Mucevher.MucevherTipi.bomba)
                    {
                        bombaBolgesiniIsaretle(new Vector2Int(x + 1, y), board.tumMucevherler[x + 1, y]);
                    }
                }
            }

            if (mucevher.posIndex.y > 0)
            {
                if (board.tumMucevherler[x , y-1] != null)
                {
                    if (board.tumMucevherler[x , y-1].tipi == Mucevher.MucevherTipi.bomba)
                    {
                        bombaBolgesiniIsaretle(new Vector2Int(x, y-1), board.tumMucevherler[x, y-1]);
                    }
                }
            }

            if (mucevher.posIndex.y < board.yuseklik-1)
            {
                if (board.tumMucevherler[x, y +1] != null)
                {
                    if (board.tumMucevherler[x, y +1].tipi == Mucevher.MucevherTipi.bomba)
                    {
                        bombaBolgesiniIsaretle(new Vector2Int(x, y+1), board.tumMucevherler[x, y + 1]);
                    }
                }
            }

        }
    }



    public void bombaBolgesiniIsaretle(Vector2Int bombapos , Mucevher bomba)
    {
        for (int x = bombapos.x - bomba.bombaHacmi; x <= bombapos.x + bomba.bombaHacmi; x++)
        {
            for (int y =bombapos.y - bomba.bombaHacmi; y < bombapos.y + bomba.bombaHacmi; y++)
            {
                if(x>=0 && x<board.genislik-1 && y>=0 && y<board.yuseklik-1)
                {
                    if (board.tumMucevherler[x,y] !=null)
                    {
                        board.tumMucevherler[x, y].eslestimi = true;
                        BulunanMucevherListe.Add(board.tumMucevherler[x, y]);
                    }
                }
            }
            if (BulunanMucevherListe.Count > 0)
            {
                BulunanMucevherListe = BulunanMucevherListe.Distinct().ToList();
            }
        }
    }
}
