using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status
{
    // ���� ���� (���� / �� ����)
    int id;             // ���� ���̵�
    int hp;             // ü��
    int mp;             // ����
    int power;          // ���ݷ�
    int defence;        // ����
    int healHPPerSec;   // �ʴ� ü�� ȸ����   
    int healMPPerSec;   // �ʴ� ���� ȸ����
    double critPercent; // ġ��Ÿ Ȯ��
    int attackSpeed;    // ���� �ӵ�(1 sec)
    
    // ���� ���� (���� (�����) ����) --> ���Ӹ޴��� ���� ����
    int goldAcquisition;    // ��� ȹ�淮(����ġ)
    int expAcquisition;     // ����ġ ȹ�淮(����ġ)
    int userTouchDamage;    // ��ġ �� ���ݷ�

}
