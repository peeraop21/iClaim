<template>
    <div class=" container" align="center">
        <div class="row">
            <div class="col-12" align="center">
                <h2 id="header2">รายละเอียดการรักษา</h2>
                <div align="left" class="txt mb-4 mt-5">
                    <p>โรงพยาบาลกรุงเทพ</p>
                    <p>วันที่เข้ารักษา : 17/06/2564</p>
                </div>
                <table id="treatments" class="mb-4">
                    <tr>
                        <th>รายการ</th>
                        <th>จำนวนเงิน</th>
                    </tr>
                    <tr  v-for="treatment in treatments" :key="treatment.id">
                        <td>{{ treatment.treatment_list }}</td>
                        <td>{{ treatment.money }} บาท</td>
                    </tr>
                </table>
                <p class="p_right">รวมจำนวนเงิน: 10000 บาท</p>

            </div>
        </div>
        <br>
     
    </div>
</template>

<style>
#treatments {
  border-collapse: collapse;
  width: 100%;
}

#treatments td, #treatments th {
  border: 1px solid #ddd;
  padding: 8px;
}

#treatments tr:nth-child(even){background-color: #f2f2f2;}

#treatments tr:hover {background-color: #ddd;}

#treatments th {
  border: 1.8px solid #ddd;
  padding-top: 12px;
  padding-bottom: 12px;
  text-align: center;
  background-color: #5c2e91;
  color: white;
}
</style>

<script>
export default {
  data () {
      return {
          treatments: [
              {
                  id: 1,
                  treatment_list: "ค่ายาและสารบำบัด",
                  money: 17000
              },
              {
                  id: 2,
                  treatment_list: "ค่าอวัยวะเทียม",
                  money: 0
              },
              {
                  id: 3,
                  treatment_list: "ค่าบริการทางการแพทย์",
                  money: 1000
              },
              {
                  id: 4,
                  treatment_list: "ค่าห้องและค่าอาหาร",
                  money: 500
              },
              {
                  id: 5,
                  treatment_list: "ค่าพาหนะและค่านำส่งสถานพยาบาล",
                  money: 500
              },
          ],
          msg: "ดูเพิ่มเติม",
          msg2: "เบิกค่ารักษาเบื้องต้น"
      }
  }
}
</script>