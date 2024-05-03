using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int genislik;
    public int yuseklik;

    public GameObject tilePrefab;

    public Mucevher[] mucevherler;

    public Mucevher[,] tumMucevherler;

    public float mucevherHiz;
    public EslesmeController eslesmeController;

    public enum BoardDurum {bekliyor , hareketEdiyor};
    public BoardDurum gecerliDurum = BoardDurum.hareketEdiyor;

    public Mucevher bomba;
    public float bombaCikmaSansi = 2f;

    private void Awake()
    {
        eslesmeController = Object.FindObjectOfType<EslesmeController>();

    }
    private void Start()
    {
        tumMucevherler = new Mucevher[genislik, yuseklik];
        DuzenleFNC();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
           BoardKaristirFNC();
        }
    }



    void DuzenleFNC()
    {
        for (int x = 0; x < genislik; x++)
        {
            for (int y = 0; y < yuseklik; y++)
            {
                Vector2 pos = new Vector2(x, y);

                GameObject bgTile = Instantiate(tilePrefab, pos, Quaternion.identity);

                bgTile.transform.parent = this.transform;
                bgTile.name = "BG Tile - " + x + " , " + y;

                int rastgeleMucevher = Random.Range(0, mucevherler.Length);

                int kontolSayaci = 0;
                while (EslesmeVarmiFNC(new Vector2Int(x, y), mucevherler[rastgeleMucevher]) && kontolSayaci < 100)
                {
                    rastgeleMucevher = Random.Range(0, mucevherler.Length);

                    kontolSayaci++;
                    if (kontolSayaci > 0)
                    {
                        print(kontolSayaci);
                    }
                }

                MucevherOlustur(new Vector2Int(x, y), mucevherler[rastgeleMucevher]);
            }
        }
    }

    void MucevherOlustur(Vector2Int pos, Mucevher olusacakMucevher)
    {
        if(Random.Range(0f , 100f) < bombaCikmaSansi)
        {
            olusacakMucevher = bomba;
        }

        Mucevher mucevher = Instantiate(olusacakMucevher, new Vector3(pos.x, pos.y+yuseklik, 0f), Quaternion.identity);
        mucevher.transform.parent = this.transform;
        mucevher.name = "Mucevher - " + pos.x + " , " + pos.y;

        tumMucevherler[pos.x, pos.y] = mucevher;

        mucevher.MucevheriDuzenle(pos, this);
    }

    bool EslesmeVarmiFNC(Vector2Int posKontrol, Mucevher kontrolEdilenMucevher)
    {
        if (posKontrol.x > 1)
        {
            if (tumMucevherler[posKontrol.x - 1, posKontrol.y].tipi == kontrolEdilenMucevher.tipi && tumMucevherler[posKontrol.x - 2, posKontrol.y].tipi == kontrolEdilenMucevher.tipi)
            {
                return true;
            }
        }
        if (posKontrol.y > 1)
        {
            if (tumMucevherler[posKontrol.x, posKontrol.y - 1].tipi == kontrolEdilenMucevher.tipi && tumMucevherler[posKontrol.x, posKontrol.y - 2].tipi == kontrolEdilenMucevher.tipi)
            {
                return true;
            }
        }

        return false;
    }


    void EslesenMucevheriYokEt(Vector2Int pos)
    {
        if (tumMucevherler[pos.x, pos.y] != null)
        {
            if (tumMucevherler[pos.x, pos.y].eslestimi)
            {

                if (tumMucevherler[pos.x , pos.y].tipi == Mucevher.MucevherTipi.bomba)
                {
                    soundManager.Instance.PatlamaSesiCikar();
                }else
                {
                    soundManager.Instance.MucevherSesiCikar();
                }

                Instantiate(tumMucevherler[pos.x, pos.y].mucevherEfekt, new Vector2(pos.x, pos.y), Quaternion.identity);
                Destroy(tumMucevherler[pos.x, pos.y].gameObject);
                tumMucevherler[pos.x, pos.y] = null;


            }
        }
    }

    public void TumEslesenleriYokEt()
    {
        for (int i = 0; i < eslesmeController.BulunanMucevherListe.Count; i++)
        {
            if (eslesmeController.BulunanMucevherListe[i] != null)
            {
                UIManager.Instance.PuaniArtirFNC(eslesmeController.BulunanMucevherListe[i].skorDegeri);
                EslesenMucevheriYokEt(eslesmeController.BulunanMucevherListe[i].posIndex);

            }
        }

        StartCoroutine(AltaKaydirRouitine());
    }

    IEnumerator AltaKaydirRouitine()
    {
        yield return new WaitForSeconds(.2f);
        int bosSayac = 0;

        for (int x = 0; x < genislik; x++)
        {
            for (int y = 0; y < yuseklik; y++)
            {
                if (tumMucevherler[x, y] == null)
                {
                    bosSayac++;
                }
                else if (bosSayac > 0)
                {
                    tumMucevherler[x, y].posIndex.y -= bosSayac;
                    tumMucevherler[x, y - bosSayac] = tumMucevherler[x, y];
                    tumMucevherler[x, y] = null;
                }

            }
            bosSayac = 0;
        }
        StartCoroutine(BoardYenidenDoldurRouitine());
    }

    IEnumerator BoardYenidenDoldurRouitine()
    {
        yield return new WaitForSeconds(.5f);
        ustBosluklariDoldur();

        yield return new WaitForSeconds(0.5f);

        eslesmeController.EslesmeleriBulFNC();

        if (eslesmeController.BulunanMucevherListe.Count > 0)
        {
            yield return new WaitForSeconds(0.5f);
            TumEslesenleriYokEt();
        }else
        {
            yield return new WaitForSeconds(.5f);
            gecerliDurum = BoardDurum.hareketEdiyor;
        }
    }

    void ustBosluklariDoldur()
    {
        for (int x = 0; x < genislik; x++)
        {
            for (int y = 0; y < yuseklik; y++)
            {
                if (tumMucevherler[x, y] == null)
                {
                    int rastgeleMucevher = Random.Range(0, mucevherler.Length);

                    MucevherOlustur(new Vector2Int(x, y), mucevherler[rastgeleMucevher]);
                }

            }
        }
        YanlisYerlestirmeKontrolEt();
    }

    void YanlisYerlestirmeKontrolEt()
    {
        List<Mucevher> bulunanMucevherList = new List<Mucevher>();

        bulunanMucevherList.AddRange(FindObjectsOfType<Mucevher>());
        for (int x = 0; x < genislik; x++)
        {
            for (int y = 0; y < yuseklik; y++)
            {
                if (bulunanMucevherList.Contains(tumMucevherler[x,y]))
                {
                    bulunanMucevherList.Remove(tumMucevherler[x, y]);
                }
            }

        }
        foreach (Mucevher mucevher in bulunanMucevherList)
        {
            Destroy(mucevher.gameObject);
        }
    }

    public void BoardKaristirFNC()
    {
        if (gecerliDurum != BoardDurum.bekliyor)
        {
            gecerliDurum = BoardDurum.bekliyor;

            List<Mucevher> sahnedekiMucevherlerList = new List<Mucevher>();

            for (int x = 0; x < genislik; x++)
            {
                for (int y = 0; y < yuseklik; y++)
                {
                    sahnedekiMucevherlerList.Add(tumMucevherler[x, y]);
                    tumMucevherler[x, y] = null;
                }
            }

            for (int x = 0; x < genislik; x++)
            {
                for (int y = 0; y < yuseklik; y++)
                {
                    int kulanilacakMucevher = Random.Range(0, sahnedekiMucevherlerList.Count);

                    int kontrolSayac = 0;

                    while (EslesmeVarmiFNC(new Vector2Int(x, y), sahnedekiMucevherlerList[kulanilacakMucevher]) && kontrolSayac<100 && sahnedekiMucevherlerList.Count >1)
                    {
                        kulanilacakMucevher = Random.Range(0, sahnedekiMucevherlerList.Count);
                        kontrolSayac++;
                    }

                    sahnedekiMucevherlerList[kulanilacakMucevher].MucevheriDuzenle(new Vector2Int(x, y), this);
                    tumMucevherler[x, y] = sahnedekiMucevherlerList[kulanilacakMucevher];
                    sahnedekiMucevherlerList.RemoveAt(kulanilacakMucevher);
                }
            }

            StartCoroutine(AltaKaydirRouitine());
        }

    }

}
