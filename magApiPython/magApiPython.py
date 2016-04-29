#!usr/bin/python
#coding=utf-8

import httplib, urllib, base64
import datetime
import json
from collections import OrderedDict
import random
headers = {
    # Request headers
    'Ocp-Apim-Subscription-Key': 'f7cc29509a8443c5b3a5e56b0e38b5a6',
}
#test for id
class magApi:
    dataStr=''
    successOrNot=False
    error=Exception() 
    conn = httplib.HTTPSConnection('oxfordhk.azure-api.net')

    def evaluate(self,expr,count=10,offset=0,attributes='Id,F.FId,AA.AuId,AA.AfId,RId,J.JId,C.CId'):
        '''
        @note: use amg_api 'evaluate' to get dataStr
        @param expr, count(option, default 10), offset(option, default 0), attribute(option, default'Id,F.FId,AA.AuId,AA.AfId,RId,J.JId,C.CId')
        @rtype: bool, return True means successed, False means failed
        '''
        params = urllib.urlencode({
        # Request parameters
        'expr': expr,
        'model': 'latest',
        'count': count,
        'offset': offset,
        'orderby': 'Id:asc',
        'attributes': attributes,
        })
        try:
            self.conn.request("GET", "/academic/v1.0/evaluate?%s" % params, "{body}", headers)
            response = self.conn.getresponse()
            self.successOrNot=True
            self.dataStr=response.read()
        except Exception as e:
            successOrNot=False
            error=e
        return self.successOrNot
    #exchange str to dict (json)
    def dataDict(self):
        '''
        @note: exchange data from str to dict. (json)
        @rtype: dict
        '''
        return json.loads(self.dataStr)
        
if(__name__=='__main__'):
    print 'test for Id'
    
    # example here
    
    test=magApi()
    for i in xrange(100):
        #print i
        T1=datetime.datetime.now()
        test.evaluate(expr=''.join(['Id=',str(random.randint(0,1000000)+2171526461)]),count=10000,offset=0)
        if(test.successOrNot==True):
            print i
            #print test.dataStr
            #exchange str to dict
            print test.dataDict()
            T2=datetime.datetime.now()
            print 'time:', T2-T1
        else:
            print("[Errno {0}] {1}".format(test.error.errno, test.error.strerror))
