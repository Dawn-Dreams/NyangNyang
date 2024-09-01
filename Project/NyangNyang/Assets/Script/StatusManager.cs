using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status
{
    // ���� ���� (���� / �� ����)
    public int id;             // ���� ���̵�
    public int hp;             // ü��
    public int mp;             // ����
    public int power;          // ���ݷ�
    public int defence;        // ����
    public int healHPPerSec;   // �ʴ� ü�� ȸ����   
    public int healMPPerSec;   // �ʴ� ���� ȸ����
    public double critPercent; // ġ��Ÿ Ȯ��
    public int attackSpeed;    // ���� �ӵ�(1 sec)
    
    // ���� ���� (���� (�����) ����) --> ���Ӹ޴��� ���� ����
    int goldAcquisition;    // ��� ȹ�淮(����ġ)
    int expAcquisition;     // ����ġ ȹ�淮(����ġ)
    int userTouchDamage;    // ��ġ �� ���ݷ�

}
