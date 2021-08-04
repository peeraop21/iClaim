<template>
    <div class=" container space-contianer" align="center">
        <div class="row">
            <div class="col-12" align="center">
                <h2 id="header2">สิทธิ์การรักษาเบื้องต้น</h2>
                <br>
                <div align="left" class="txt">
                    <p>ชื่อ-สกุล: นายรำพี พีรำรำ</p>
                    <p>เลขบัตรประจำตัวประชาชน: 1-2299-00876-32-1</p>
                    <p>ทะเบียนรถที่เกิดเหตุ : กค 5755</p>
                    <p>วันที่เกิดเหตุ : 17/06/2564</p>
                </div>
                <br>
                <p class="p_right">สิทธิ์คงเหลือ: 10000 บาท</p>
                <router-link class="btn-next" to="/Claim">เบิกค่ารักษาเบื้องต้น</router-link>
                <br><br>
                <div align="left" style="width: 90%;">
                    <label>ประวัติการรักษา</label><br>
                </div>
                <section>
                    <div style="height: 100%; width: 90%;">
                        <div class="accordion" v-for="boto in boto_" :key="boto.id">
                            <div class="accordion-item" :id="'list' + boto.id">
                                <a class="accordion-link" :href="'#list' + boto.id">
                                    <div>
                                        <p>
                                            <ion-icon name="document-text-outline"></ion-icon>{{ boto.boto_no }}<br>
                                            <ion-icon name="card-outline"></ion-icon>จำนวนเงิน: {{ boto.money }} บาท
                                        </p>
                                    </div>
                                    <ion-icon name="chevron-down-outline" class="icon ion-md-add"></ion-icon>
                                </a>
                                <div class="answer">
                                    <p>
                                        วันที่: {{ boto.date }}<br>
                                        โรงพยาบาลที่รักษา: {{ boto.hospital }}
                                    </p>
                                </div>
                                <div style="text-align: center">
                                    <router-link class="btn-select" to="/RightsHistoryDetail">ดูเพิ่มเติม</router-link>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>

            </div>
        </div>
        <br>
     
    </div>
</template>

<script>
export default {
  name: 'RightsHistory',
  data () {
      return {
          boto_: [
              {
                  id: 1,
                  boto_no: "บต3/XXX/64/00001",
                  date: "05/06/2564",
                  hospital: "โรงพยาบาลสำโรงการแพทย์",
                  money: 17000
              },
              {
                  id: 2,
                   boto_no: "บต3/XXX/64/00002",
                  date: "18/06/2564",
                  hospital: "โรงพยาบาลกรุงเทพ",
                  money: 3000
              },
          ],
      }
  }
}
</script>

<style>

</style>
